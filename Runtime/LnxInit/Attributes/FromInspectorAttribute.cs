using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromInspectorAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }
        public string Attr { get; set; }
        public IInspectorValueGetter InspectorGetter { get; set; }

        public Component FetchOne(FetchContext ctx)
        {
            return InspectorGetter.GetOne(ctx.Behaviour);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            return InspectorGetter.GetMany(ctx.Behaviour);
        }
    }
}
