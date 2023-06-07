using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    internal class LnxServiceMandatoryParameterException : Exception
    {
        public LnxServiceMandatoryParameterException()
        {
        }

        public LnxServiceMandatoryParameterException(string message) : base(message)
        {
        }

        public LnxServiceMandatoryParameterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LnxServiceMandatoryParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
