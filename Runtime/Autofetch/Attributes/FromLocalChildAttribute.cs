using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromLocalChildAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }

        public FromLocalChildAttribute(int order = 0)
        {
            LookupOrder = order;
        }

        public Component FetchOne(FetchContext ctx)
        {
            return ctx.Behaviour.GetComponentInChildren(ctx.Type, includeInactive: true);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            return ctx.Behaviour.GetComponentsInChildren(ctx.Type, includeInactive: true);
        }
    }

}
