using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LnxArch
{
    [Serializable]
    public class LnxEntityNotFound : Exception
    {
        public LnxEntityNotFound()
        {
        }

        public LnxEntityNotFound(GameObject gameObject)
        : base(MessageFor(gameObject))
        {
        }

        public LnxEntityNotFound(Component component)
        : base(MessageFor(component))
        {
        }

        public LnxEntityNotFound(string message) : base(message)
        {
        }

        public LnxEntityNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxEntityNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string MessageFor(GameObject gameObject)
            => $"No LnxEntity found for {gameObject.name}.";

        private static string MessageFor(Component component)
            => MessageFor(component.gameObject);
    }
}
