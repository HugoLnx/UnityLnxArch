using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LnxArch
{
    [Serializable]
    public class LnxServiceEntityNestedException : Exception
    {
        private readonly MonoBehaviour _parent;
        private readonly LnxServiceEntity _child;

        public LnxServiceEntityNestedException()
        {
        }

        public LnxServiceEntityNestedException(string message) : base(message)
        {
        }

        public LnxServiceEntityNestedException(MonoBehaviour parent, LnxServiceEntity child)
        : base($"LnxServiceEntity can NOT be nested. {parent.gameObject.name} > {child.gameObject.name}")
        {
            _parent = parent;
            _child = child;
        }

        public LnxServiceEntityNestedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxServiceEntityNestedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
