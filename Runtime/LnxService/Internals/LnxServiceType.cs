using System;
using System.Reflection;

namespace LnxArch
{
    public class LnxServiceType
    {
        private bool _forcedAutoAdd;
        public Type Type { get; private set; }
        public LnxServiceAttribute Attribute { get; private set; }
        public InitType InitType { get; private set; }
        public bool IsPersistent => Attribute.Persistent;
        public bool IsAutoAdd => Attribute.AutoAdd || _forcedAutoAdd;
        public bool HasInitMethods => InitType != null;

        public LnxServiceType(Type type, LnxServiceAttribute attribute, InitType initType)
        {
            Type = type;
            Attribute = attribute;
            InitType = initType;
        }

        public static LnxServiceType TryToBuildFrom(Type type, InitType initType = null)
        {
            LnxServiceAttribute attribute = type.GetCustomAttribute<LnxServiceAttribute>();
            if (attribute == null) return null;
            return new LnxServiceType(type, attribute, initType);
        }

        public void ForceAutoAdd()
        {
            _forcedAutoAdd = true;
        }
    }
}
