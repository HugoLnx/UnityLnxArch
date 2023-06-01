using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace LnxArch
{
    [Serializable]
    internal class CouldNotFindAttributeOrPropertyForFromInspector : Exception
    {

        public CouldNotFindAttributeOrPropertyForFromInspector()
        {
        }

        public CouldNotFindAttributeOrPropertyForFromInspector(string message) : base(message)
        {
        }

        public CouldNotFindAttributeOrPropertyForFromInspector(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CouldNotFindAttributeOrPropertyForFromInspector(AutofetchType declaringType, ParameterInfo parameter, FromInspectorAttribute attribute)
            : base($"Could not find attribute or property for [{attribute.GetType().Name} attr={attribute.Attr}] {parameter.Name} on {declaringType.Type.Name}")
        {
        }

        protected CouldNotFindAttributeOrPropertyForFromInspector(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
