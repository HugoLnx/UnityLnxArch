using System;
using System.Reflection;

namespace LnxArch
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class LnxAutoAddAttribute : Attribute
    {
        public AutoAddTarget Target { get; set; } = AutoAddTarget.Entity;

        public static LnxAutoAddAttribute GetFrom(Type type)
        {
            return (LnxAutoAddAttribute) Attribute.GetCustomAttribute(type, typeof(LnxAutoAddAttribute), true);
        }
        public static LnxAutoAddAttribute GetFrom(ParameterInfo param)
        {
            return (LnxAutoAddAttribute) Attribute.GetCustomAttribute(param, typeof(LnxAutoAddAttribute), true);
        }
    }
}
