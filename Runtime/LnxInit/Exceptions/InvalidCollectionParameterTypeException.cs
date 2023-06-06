using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    internal class InvalidCollectionParameterTypeException : Exception
    {
        private InitMethodParameter _param;

        public InvalidCollectionParameterTypeException()
        {
        }

        public InvalidCollectionParameterTypeException(InitMethodParameter param)
        : base($"Error on parameter {param.ToHumanName()} : Only types derived from Component, Component[] or List<Component> can be auto fetched.")
        {
            _param = param;
        }

        public InvalidCollectionParameterTypeException(string message) : base(message)
        {
        }

        public InvalidCollectionParameterTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCollectionParameterTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
