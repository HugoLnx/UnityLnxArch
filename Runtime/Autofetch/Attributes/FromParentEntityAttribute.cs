using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromParentEntityAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }

        public FromParentEntityAttribute(int order = 0)
        {
            LookupOrder = order;
        }

        public Component FetchOne(MonoBehaviour _, LnxEntity entity, Type type)
        {
            LnxEntity parentEntity = entity.transform.parent.GetComponentInParent<LnxEntity>(includeInactive: true);
            return parentEntity.FetchFirst(type);
        }
        public IEnumerable<Component> FetchMany(MonoBehaviour _, LnxEntity entity, Type type)
        {
            LnxEntity parentEntity = entity.transform.parent.GetComponentInParent<LnxEntity>(includeInactive: true);
            return parentEntity.FetchAll(type);
        }
    }

}
