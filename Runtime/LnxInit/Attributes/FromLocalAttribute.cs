using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromLocalAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }

        public FromLocalAttribute(int order = 0)
        {
            LookupOrder = order;
        }

        public Component FetchOne(FetchContext ctx)
        {
            return ctx.Behaviour.GetComponent(ctx.Type);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            return ctx.Behaviour.GetComponents(ctx.Type);
        }
    }

}
