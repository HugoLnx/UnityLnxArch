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
        public InitType DeclaringType { get; set; }
        private readonly object[] _valuesBuffer;

        public InitMethod(MethodInfo method, LnxInitAttribute initAttribute,
            InitMethodParameter[] parameters)
        {
            Info = method;
            InitAttribute = initAttribute;
            Parameters = parameters;
            _valuesBuffer = new object[parameters.Length];
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
    }
}
