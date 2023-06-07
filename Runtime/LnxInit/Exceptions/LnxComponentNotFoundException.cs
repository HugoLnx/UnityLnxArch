using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    public class LnxComponentNotFoundException : Exception
    {
        public LnxComponentNotFoundException()
        {
        }

        public LnxComponentNotFoundException(string message) : base(message)
        {
        }

        public LnxComponentNotFoundException(Type type, LnxEntity entity)
        : base($"Could not fetch {type.Name} from entity {entity.gameObject.name}.")
        {
        }

        public LnxComponentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxComponentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
