using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    public struct FetchContext
    {
        public MonoBehaviour Behaviour;
        public LnxEntity Entity;
        public Type Type;
        public InitMethodParameter Parameter;
    }
    public interface IFetchAttribute
    {
        int LookupOrder { get; set; }
        Component FetchOne(FetchContext ctx);
        IEnumerable<Component> FetchMany(FetchContext ctx);
    }

}
