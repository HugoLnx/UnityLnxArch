using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    internal class GenericInitMethodException : Exception
    {
        private InitMethod _method;

        public GenericInitMethodException()
        {
        }

        public GenericInitMethodException(InitMethod method)
        : base($"Method {method.ToHumanName()} can not be generic.")
        {
            _method = method;
        }

        public GenericInitMethodException(string message) : base(message)
        {
        }

        public GenericInitMethodException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GenericInitMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
