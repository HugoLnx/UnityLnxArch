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

        public Component FetchOne(FetchContext ctx)
        {
            LnxEntity parentEntity = LnxEntity.FetchEntityParentOf(ctx.Entity);
            return parentEntity.FetchFirst(ctx.Type);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            LnxEntity parentEntity = LnxEntity.FetchEntityParentOf(ctx.Entity);
            return parentEntity.FetchAll(ctx.Type);
        }
    }

}
