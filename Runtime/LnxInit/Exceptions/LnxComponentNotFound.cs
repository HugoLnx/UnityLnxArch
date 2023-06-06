using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    public class LnxComponentNotFound : Exception
    {
        public LnxComponentNotFound()
        {
        }

        public LnxComponentNotFound(string message) : base(message)
        {
        }

        public LnxComponentNotFound(Type type, LnxEntity entity)
        : base($"Could not fetch {type.Name} from entity {entity.gameObject.name}.")
        {
        }

        public LnxComponentNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxComponentNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
