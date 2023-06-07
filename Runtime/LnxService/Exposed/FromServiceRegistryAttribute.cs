using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromServiceRegistryAttribute : Attribute, IFetchAttribute
    {
        public int LookupOrder { get; set; }

        private readonly ServiceEntityRegistry _registry;

        public FromServiceRegistryAttribute(int order = 0)
        {
            LookupOrder = order;
            _registry = ServiceEntityRegistry.Instance;
        }

        public Component FetchOne(FetchContext ctx)
        {
            return _registry.Retrieve(ctx.Type);
        }
        public IEnumerable<Component> FetchMany(FetchContext ctx)
        {
            throw new NotImplementedException("Fetching many services are not supported.");
        }
    }
}
