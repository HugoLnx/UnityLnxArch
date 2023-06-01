using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromLocalAncestorAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }

        public FromLocalAncestorAttribute(int order = 0)
        {
            LookupOrder = order;
        }

        public Component FetchOne(FetchContext ctx)
        {
            return ctx.Behaviour.GetComponentInParent(ctx.Type, includeInactive: true);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            return ctx.Behaviour.GetComponentsInParent(ctx.Type, includeInactive: true);
        }
    }

}
