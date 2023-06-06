using System;
using System.Collections.Generic;
using System.Linq;

namespace LnxArch
{
    public sealed class TypesPreProcessor
    {
        public static TypesPreProcessor Instance { get; } = new TypesPreProcessor();
        private IEnumerable<Type> _allTypes;
        private Dictionary<Type, InitType> _mapInitType;
        private Dictionary<Type, LnxServiceType> _mapServiceType;

        private IEnumerable<Type> AllTypes =>
            _allTypes ??=
            AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes());

        private TypesPreProcessor() {
            PreProcessTypes();
        }

        public bool IsInitType(Type type)
        {
            return _mapInitType.ContainsKey(type);
        }

        public InitType GetInitTypeOf(Type type)
        {
            return _mapInitType.GetValueOrDefault(type);
        }

        public bool IsServiceType(Type type)
        {
            return _mapServiceType.ContainsKey(type);
        }

        public LnxServiceType GetServiceType(Type type)
        {
            return _mapServiceType.GetValueOrDefault(type);
        }

        private void PreProcessTypes()
        {
            _mapInitType = new Dictionary<Type, InitType>();
            _mapServiceType = new Dictionary<Type, LnxServiceType>();
            foreach (Type type in AllTypes)
            {
                InitType initType = InitType.TryToBuildFrom(type);
                if (initType != null) _mapInitType.Add(type, initType);
                LnxServiceType serviceType = LnxServiceType.TryToBuildFrom(type, initType);
                if (serviceType != null) _mapServiceType.Add(type, serviceType);
            }
        }
    }
}
