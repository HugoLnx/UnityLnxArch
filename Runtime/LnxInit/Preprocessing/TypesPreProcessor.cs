using System;
using System.Collections.Generic;
using System.Linq;

namespace LnxArch
{
    public sealed class TypesPreProcessor
    {
        public static TypesPreProcessor Instance { get; } = new();
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
            AutoAddRegistry autoAddRegistry = new();
            _mapInitType = new Dictionary<Type, InitType>();
            _mapServiceType = new Dictionary<Type, LnxServiceType>();
            foreach (Type type in AllTypes)
            {
                InitType initType = InitType.TryToBuildFrom(type);
                if (initType != null) _mapInitType.Add(type, initType);
                LnxServiceType serviceType = LnxServiceType.TryToBuildFrom(type, initType);
                if (serviceType == null)
                {
                    // TODO: If it has LnxAutoAdd attribute add to registry
                }
                else
                {
                    _mapServiceType.Add(type, serviceType);
                    // TODO: If it has LnxAutoAdd attribute serviceType.IsAutoAdd should be true
                    if (serviceType.IsAutoAdd)
                    {
                        autoAddRegistry.Register(type, AutoAddType.Service);
                    }
                }
            }
            LinkAutoAddToParams(autoAddRegistry);
        }

        private void LinkAutoAddToParams(AutoAddRegistry autoAddRegistry)
        {
            foreach (InitMethodParameter initParam in AllInitParameters())
            {
                initParam.AutoAddExecutor = autoAddRegistry.Retrieve(initParam.Type);
            }
        }

        private IEnumerable<InitMethodParameter> AllInitParameters()
        {
            foreach (InitType initType in _mapInitType.Values)
            {
                foreach (InitMethod initMethod in initType.InitMethods)
                {
                    foreach (InitMethodParameter initParam in initMethod.Parameters)
                    {
                        yield return initParam;
                    }
                }
            }
        }
    }
}
