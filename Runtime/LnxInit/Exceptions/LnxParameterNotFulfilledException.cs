using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    internal class LnxParameterNotFulfilledException : Exception
    {
        private InitMethodParameter param;

        public LnxParameterNotFulfilledException()
        {
        }

        public LnxParameterNotFulfilledException(InitMethodParameter param)
        : base(param.ToHumanName())
        {
            this.param = param;
        }

        public LnxParameterNotFulfilledException(string message) : base(message)
        {
        }

        public LnxParameterNotFulfilledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxParameterNotFulfilledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
