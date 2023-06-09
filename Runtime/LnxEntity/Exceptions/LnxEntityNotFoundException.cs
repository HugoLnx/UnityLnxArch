using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LnxArch
{
    [Serializable]
    public class LnxEntityNotFoundException : Exception
    {
        public LnxEntityNotFoundException()
        {
        }

        public LnxEntityNotFoundException(GameObject gameObject)
        : base(MessageFor(gameObject))
        {
        }

        public LnxEntityNotFoundException(Component component)
        : base(MessageFor(component))
        {
        }

        public LnxEntityNotFoundException(string message) : base(message)
        {
        }

        public LnxEntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxEntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string MessageFor(GameObject gameObject)
            => $"No LnxEntity found for {gameObject.name}.";

        private static string MessageFor(Component component)
            => MessageFor(component.gameObject);
    }
}
