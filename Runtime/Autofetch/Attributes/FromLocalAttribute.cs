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

        public Component FetchOne(MonoBehaviour behaviour, LnxEntity _, Type type)
        {
            return behaviour.GetComponent(type);
        }
        public IEnumerable<Component> FetchMany(MonoBehaviour behaviour, LnxEntity _, Type type)
        {
            return behaviour.GetComponents(type);
        }
    }

}
