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
        : base($"Behaviour {behaviour.gameObject.name}/{behaviour.GetType().Name} must be inside a LnxEntity or LnxServiceEntity because it has InitMethods.")
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
