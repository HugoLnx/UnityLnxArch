using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            Dictionary<Type, AutoAddTarget?> autoAddRegistry = new();
            AutoAddExecutorFactory autoAddExecutorFactory = new();
            InitTypeFactory initTypeFactory = InitTypeFactory.BuildFactoriesHierarchy();

            _mapInitType = new Dictionary<Type, InitType>();
            _mapServiceType = new Dictionary<Type, LnxServiceType>();
            foreach (Type type in AllTypes)
            {
                InitType initType = initTypeFactory.TryToBuildFrom(type);
                if (initType != null) _mapInitType.Add(type, initType);
                LnxServiceType serviceType = LnxServiceType.TryToBuildFrom(type, initType);
                LnxAutoAddAttribute autoAddAttr = LnxAutoAddAttribute.GetFrom(type);
                bool isService = serviceType != null;
                if (isService)
                {
                    if (autoAddAttr != null)
                    {
                        serviceType.ForceAutoAdd();
                    }
                    _mapServiceType.Add(type, serviceType);
                    if (serviceType.IsAutoAdd)
                    {
                        autoAddRegistry.Add(type, AutoAddTarget.Service);
                    }
                }
                else if (autoAddAttr != null)
                {
                    if (autoAddAttr.Target == AutoAddTarget.Service)
                    {
                        throw new InvalidAutoAddTargetException($"Type {type.Name} has LnxAutoAdd.Target=Service but it is not a LnxService.");
                    }
                    autoAddRegistry.Add(type, autoAddAttr.Target);
                }
            }
            LinkAutoAddToParams(autoAddRegistry, autoAddExecutorFactory);
            ValidatePersistentServiceDependencies();
        }

        private void LinkAutoAddToParams(Dictionary<Type, AutoAddTarget?> autoAddRegistry, AutoAddExecutorFactory executorFactory)
        {
            foreach (InitMethodParameter initParam in AllInitParameters())
            {
                bool isService = _mapServiceType.ContainsKey(initParam.Type);
                AutoAddTarget? maybeTarget = LnxAutoAddAttribute.GetFrom(initParam.Info)?.Target
                    ?? autoAddRegistry.GetValueOrDefault(initParam.Type);
                if (!maybeTarget.HasValue)
                {
                    if (isService && !initParam.Info.HasDefaultValue)
                    {
                        throw new LnxServiceMandatoryParameterException(
                            $"{initParam.ToHumanName()}: LnxService parameters with AutoAdd disabled need default value."
                            + $" Consider changing it to {initParam.Type.Name} {initParam.Info.Name} = null");
                    }
                    continue;
                }
                AutoAddTarget target = maybeTarget.Value;

                if (!isService && target == AutoAddTarget.Service)
                {
                    throw new InvalidAutoAddTargetException(
                        $"{initParam.ToHumanName()} has LnxAutoAdd.Target={target}, "
                        + $"but type {initParam.DeclaringType.Type.Name} is a LnxService.");
                }

                initParam.AutoAddExecutor = executorFactory.ExecutorFor(target);
            }
        }

        private AutoAddExecutor ExecutorFromParameterAttribute(ParameterInfo param, AutoAddExecutorFactory factory)
        {
            LnxAutoAddAttribute autoAddAttr = LnxAutoAddAttribute.GetFrom(param);
            if (autoAddAttr == null) return null;
            return factory.ExecutorFor(autoAddAttr.Target);
        }

        private IEnumerable<InitMethodParameter> AllInitParameters()
        {
            foreach (InitType initType in _mapInitType.Values)
            {
                foreach (InitMethodParameter initParam in AllInitParametersOf(initType))
                {
                    yield return initParam;
                }
            }
        }
        private IEnumerable<InitMethodParameter> AllInitParametersOf(InitType initType)
        {
            foreach (InitMethod initMethod in initType.InitMethods)
            {
                foreach (InitMethodParameter initParam in initMethod.Parameters)
                {
                    yield return initParam;
                }
            }
        }

        private void ValidatePersistentServiceDependencies()
        {
            foreach (LnxServiceType serviceType in _mapServiceType.Values)
            {
                if (!serviceType.IsPersistent || !serviceType.HasInitMethods) continue;
                ValidateIfParametersCanBelongToPersistentService(AllInitParametersOf(serviceType.InitType));
            }
        }

        private void ValidateIfParametersCanBelongToPersistentService(IEnumerable<InitMethodParameter> initParams)
        {
            foreach (InitMethodParameter initParam in initParams)
            {
                LnxServiceType serviceType = _mapServiceType.GetValueOrDefault(initParam.Type);
                if (serviceType == null) continue;
                if (serviceType.IsPersistent) continue;

                throw new InvalidDepedencyForPersistentServiceException(initParam);
            }
        }
    }
}
