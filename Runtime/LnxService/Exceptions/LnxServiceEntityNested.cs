using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LnxArch
{
    [Serializable]
    public class LnxServiceEntityNested : Exception
    {
        private readonly MonoBehaviour _parent;
        private readonly LnxServiceEntity _child;

        public LnxServiceEntityNested()
        {
        }

        public LnxServiceEntityNested(string message) : base(message)
        {
        }

        public LnxServiceEntityNested(MonoBehaviour parent, LnxServiceEntity child)
        : base($"LnxServiceEntity can NOT be nested. {parent.gameObject.name} > {child.gameObject.name}")
        {
            _parent = parent;
            _child = child;
        }

        public LnxServiceEntityNested(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxServiceEntityNested(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
