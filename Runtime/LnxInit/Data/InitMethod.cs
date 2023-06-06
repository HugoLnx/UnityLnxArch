using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace LnxArch
{
    public class InitMethod
    {
        public MethodInfo Info { get; }
        public LnxInitAttribute InitAttribute { get; }
        public InitMethodParameter[] Parameters { get; private set; }
        public InitType DeclaringType { get; }

        private object[] _valuesBuffer;

        public InitMethod(MethodInfo method, LnxInitAttribute initAttribute,
            InitType declaringType, InitMethodParameter[] parameters = null)
        {
            Info = method;
            InitAttribute = initAttribute;
            DeclaringType = declaringType;
            if (parameters != null) SetupParameters(parameters);
        }

        public static InitMethod BuildFrom(MethodInfo method, InitType declaringType)
        {
            LnxInitAttribute initAttribute = method.GetCustomAttribute<LnxInitAttribute>(false);
            if (initAttribute == null) return null;
            InitMethod initMethod = new(
                method: method,
                initAttribute: initAttribute,
                declaringType: declaringType
            );
            initMethod.SetupParameters(
                method
                .GetParameters()
                .Select(param => InitMethodParameter.BuildFrom(param, declaringType, initMethod))
                .ToArray()
            );

            if (method.IsGenericMethod)
            {
                throw new GenericInitMethodException(initMethod);
            }
            return initMethod;
        }

        public IEnumerable<Type> FindTypeDependencies()
        {
            return Parameters
                .Select(param => param.ComponentType)
                .Distinct();
        }

        public void InvokeWithValues(MonoBehaviour behaviour, List<Component>[] values)
        {
            int valuesCount = values?.Length ?? 0;
            Assert.IsNotNull(behaviour);
            Assert.AreEqual(Parameters.Length, valuesCount);

            if (valuesCount == 0)
            {
                this.Info.Invoke(behaviour, null);
                return;
            }

            for (int i = 0; i < valuesCount; i++)
            {
                InitMethodParameter param = Parameters[i];
                List<Component> fetchedComponents = values[i];
                _valuesBuffer[i] = param.AdaptToBeValueOnInvokeParameter(fetchedComponents);
            }
            this.Info.Invoke(behaviour, _valuesBuffer);
        }

        public string ToHumanName()
        {
            return $"{DeclaringType.Type.Name}#{Info.Name}";
        }

        private void SetupParameters(InitMethodParameter[] parameters)
        {
            Parameters = parameters;
            _valuesBuffer = new object[parameters.Length];
        }
    }
}
