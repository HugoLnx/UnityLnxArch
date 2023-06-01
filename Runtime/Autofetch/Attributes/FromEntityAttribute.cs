using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromEntityAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }

        public FromEntityAttribute(int order = 0)
        {
            this.LookupOrder = order;
        }

        public Component FetchOne(FetchContext ctx)
        {
            return ctx.Entity.FetchFirst(ctx.Type);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            return ctx.Entity.FetchAll(ctx.Type);
        }
    }
}
