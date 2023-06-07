using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException()
        {
        }

        public ComponentNotFoundException(string message) : base(message)
        {
        }

        public ComponentNotFoundException(Type type, LnxEntity entity)
        : base($"Could not fetch {type.Name} from entity {entity.gameObject.name}.")
        {
        }

        public ComponentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ComponentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
