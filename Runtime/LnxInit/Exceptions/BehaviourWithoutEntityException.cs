using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LnxArch
{
    [Serializable]
    internal class BehaviourWithoutEntityException : Exception
    {
        private MonoBehaviour behaviour;

        public BehaviourWithoutEntityException()
        {
        }

        public BehaviourWithoutEntityException(MonoBehaviour behaviour)
        : base($"{behaviour.gameObject.name}/{behaviour.GetType().Name} must be inside a LnxEntity to have InitMethods.")
        {
            this.behaviour = behaviour;
        }

        public BehaviourWithoutEntityException(string message) : base(message)
        {
        }

        public BehaviourWithoutEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BehaviourWithoutEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
