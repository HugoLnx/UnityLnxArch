using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    public class InvalidDepedencyForPersistentServiceException : Exception
    {
        private readonly InitMethodParameter initParam;

        public InvalidDepedencyForPersistentServiceException()
        {
        }

        public InvalidDepedencyForPersistentServiceException(InitMethodParameter initParam)
        : base($"{initParam.ToHumanName()}: Persistent services can't depend on non-persistent services unless they have AutoAdd enabled.")
        {
            this.initParam = initParam;
        }

        public InvalidDepedencyForPersistentServiceException(string message) : base(message)
        {
        }

        public InvalidDepedencyForPersistentServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDepedencyForPersistentServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
