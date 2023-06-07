using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    internal class CouldNotFindAttributeOrPropertyForFromInspectorException : Exception
    {

        public CouldNotFindAttributeOrPropertyForFromInspectorException()
        {
        }

        public CouldNotFindAttributeOrPropertyForFromInspectorException(string message) : base(message)
        {
        }

        public CouldNotFindAttributeOrPropertyForFromInspectorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CouldNotFindAttributeOrPropertyForFromInspectorException(InitType declaringType, ParameterInfo parameter, FromInspectorAttribute attribute)
            : base($"Could not find attribute or property for [{attribute.GetType().Name} attr={attribute.Attr}] {parameter.Name} on {declaringType.Type.Name}")
        {
        }

        protected CouldNotFindAttributeOrPropertyForFromInspectorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
