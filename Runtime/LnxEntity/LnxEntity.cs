using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static LnxArch.ComponentUtility;

namespace LnxArch
{
    [DefaultExecutionOrder (ExecutionOrderConfig.LnxEntity)]
    public class LnxEntity : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        public bool WasInitialized { get; set; }
        private const bool DefaultIncludeInactive = true;
        private readonly List<LnxEntityFetchExtension> _fetchExtensions = new();
        private List<Component> componentsBuffer = new();

        private void Awake()
        {
            if (WasInitialized || CheckAwakePrevented()) return;
            ForceAwake();
        }

        internal void ForceAwake()
        {
            if (_root == null) _root = transform;
            SwapBodyWithRoot();
            InitService.Instance.InitEntity(this);
            WasInitialized = true;
        }

        private void SwapBodyWithRoot()
        {
            if (_root == transform) return;
            int rootSiblingInx = _root.GetSiblingIndex();
            transform.SetParent(_root.parent);
            _root.SetParent(null);
            transform.SetSiblingIndex(rootSiblingInx);
            var entityLink = _root.gameObject.AddComponent<LnxEntityLinkManual>();
            entityLink.ManualAwake(
                entity: this,
                whenTargetAffectsEntity: LnxEntityLink.AllEvents,
                extendEntityFetching: true
            );
        }

        public T FetchFirst<T>(bool includeInactive = DefaultIncludeInactive, bool canBeNull = true)
        where T : class
        {
            T component = GetComponentInChildren<T>(includeInactive) ?? FetchFirstOnLinks<T>(includeInactive);
            if (component == null && !canBeNull)
            {
                throw new ComponentNotFoundException(typeof(T), this);
            }
            return component;
        }

        public Component FetchFirst(Type type, bool includeInactive = DefaultIncludeInactive, bool canBeNull = true)
        {
            Component component = GetComponentInChildren(type, includeInactive) ?? FetchFirstOnLinks(type, includeInactive);
            if (component == null && !canBeNull)
            {
                throw new ComponentNotFoundException(type, this);
            }
            return component;
        }

        public T[] FetchAll<T>(bool includeInactive = DefaultIncludeInactive)
        where T : class
        {
            List<T> children = new();
            GetComponentsInChildren<T>(includeInactive, children);
            if (_fetchExtensions.Count == 0) return children.ToArray();

            List<T> buffer = new();
            foreach (LnxEntityFetchExtension fetchExtension in _fetchExtensions)
            {
                fetchExtension.GetComponentsInChildren<T>(includeInactive, buffer);
                children.AddRange(buffer);
            }
            return children.ToArray();
        }

        public Component[] FetchAll(Type type, bool includeInactive = DefaultIncludeInactive)
        {
            componentsBuffer.Clear();
            List<Component> children = componentsBuffer;
            children.AddRange(GetComponentsInChildren(type, includeInactive));
            foreach (LnxEntityFetchExtension fetchExtension in _fetchExtensions)
            {
                children.AddRange(fetchExtension.GetComponentsInChildren(type, includeInactive));
            }
            return children.ToArray();
        }

        public void AddFetchExtension(GameObject fetchExtensionObject)
        {
            var fetchExtension = fetchExtensionObject.AddComponent<LnxEntityFetchExtension>();
            fetchExtension.HeadEntity = this;
            _fetchExtensions.Add(fetchExtension);
        }

        public bool Contains(Component component)
        {
            LnxEntity entity = FetchEntityOf(component);
            while (entity != null)
            {
                if (entity == this) return true;
                entity = FetchEntityParentOf(entity);
            }
            return false;
        }

        private static LnxEntity[] FetchAncestorEntitiesOf(Component component)
        {
            List<LnxEntity> entities = new();
            LnxEntity entity = FetchEntityOf(component);
            while (entity != null)
            {
                entities.Add(entity);
                entity = FetchEntityParentOf(entity);
            }
            return entities.ToArray();
        }

        public static LnxEntity FetchEntityOf(Component component, bool canBeNull = true)
        {
            LnxEntity entity = component.GetComponentInParent<LnxEntity>(includeInactive: true)
                ?? component.GetComponentInParent<LnxEntityFetchExtension>(includeInactive: true)?.HeadEntity;
            if (entity == null && !canBeNull)
            {
                throw new LnxEntityNotFoundException(component);
            }
            return entity;
        }

        public static LnxEntity FetchEntityParentOf(LnxEntity entity)
        {
            return LnxEntity.FetchEntityOf(entity.transform.parent);
        }

        private T FetchFirstOnLinks<T>(bool includeInactive)
        where T : class
        {
            foreach (LnxEntityFetchExtension fetchExtension in _fetchExtensions)
            {
                T component = fetchExtension.GetComponentInChildren<T>(includeInactive);
                if (component != null) return component;
            }
            return null;
        }

        private Component FetchFirstOnLinks(Type type, bool includeInactive)
        {
            foreach (LnxEntityFetchExtension fetchExtension in _fetchExtensions)
            {
                Component component = fetchExtension.GetComponentInChildren(type, includeInactive);
                if (component != null) return component;
            }
            return null;
        }

        private bool CheckAwakePrevented()
        {
            return GetComponent<LnxEntityPreventAutomaticAwake>() != null;
        }
    }
}
