using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace LnxArch
{
    [Serializable]
    public class LnxServiceEntityInvalidChildrenException : Exception
    {
        private readonly LnxServiceEntity _entity;
        private readonly MonoBehaviour _transientService;
        private readonly MonoBehaviour _persistentService;

        public LnxServiceEntityInvalidChildrenException()
        {
        }

        public LnxServiceEntityInvalidChildrenException(string message) : base(message)
        {
        }

        public LnxServiceEntityInvalidChildrenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LnxServiceEntityInvalidChildrenException(LnxServiceEntity entity, MonoBehaviour transientService, MonoBehaviour persistentService)
        : base(
            $"LnxServiceEntity {entity.gameObject.name} can NOT have both"
            + $" persistent({persistentService.gameObject.name}/{persistentService.GetType().Name})"
            + $" and transient({transientService.gameObject.name}/{transientService.GetType().Name}) services"
        )
        {
            _entity = entity;
            _transientService = transientService;
            _persistentService = persistentService;
        }

        protected LnxServiceEntityInvalidChildrenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
