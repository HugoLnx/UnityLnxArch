using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    [DefaultExecutionOrder (-9997)]
    public class LnxEntity : MonoBehaviour
    {
        public bool WasInitialized { get; set; }
        private const bool DefaultIncludeInactive = true;

        private void Awake()
        {
            if (WasInitialized) return;
            InitService.Instance.InitEntity(this);
            WasInitialized = true;
        }

        public T FetchFirst<T>(bool includeInactive = DefaultIncludeInactive, bool canBeNull = true)
        where T : class
        {
            T component = GetComponentInChildren<T>(includeInactive);
            if (component == null && !canBeNull)
            {
                throw new LnxComponentNotFound(typeof(T), this);
            }
            return component;
        }

        public Component FetchFirst(Type type, bool includeInactive = DefaultIncludeInactive, bool canBeNull = true)
        {
            Component component = GetComponentInChildren(type, includeInactive);
            if (component == null && !canBeNull)
            {
                throw new LnxComponentNotFound(type, this);
            }
            return component;
        }

        public T[] FetchAll<T>(bool includeInactive = DefaultIncludeInactive)
        where T : class
        {
            return GetComponentsInChildren<T>(includeInactive);
        }

        public Component[] FetchAll(Type type, bool includeInactive = DefaultIncludeInactive)
        {
            return GetComponentsInChildren(type, includeInactive);
        }

        public static LnxEntity FetchEntityOf(Component component, bool canBeNull = true)
        {
            LnxEntity entity = component.GetComponentInParent<LnxEntity>(includeInactive: true);
            if (entity == null && !canBeNull)
            {
                throw new LnxEntityNotFound(component);
            }
            return entity;
        }

        public static LnxEntity FetchEntityParentOf(LnxEntity entity)
        {
            return LnxEntity.FetchEntityOf(entity.transform.parent);
        }
    }
}
