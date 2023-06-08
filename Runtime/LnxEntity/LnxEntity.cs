using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static LnxArch.ComponentUtility;

namespace LnxArch
{
    [DefaultExecutionOrder (-9995)]
    public class LnxEntity : MonoBehaviour
    {
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
            InitService.Instance.InitEntity(this);
            WasInitialized = true;
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
