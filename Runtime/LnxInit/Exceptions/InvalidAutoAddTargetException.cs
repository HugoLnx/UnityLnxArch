using System;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    public class InvalidAutoAddTargetException : Exception
    {
        public InvalidAutoAddTargetException()
        {
        }

        public InvalidAutoAddTargetException(string message) : base(message)
        {
        }

        public InvalidAutoAddTargetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidAutoAddTargetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
