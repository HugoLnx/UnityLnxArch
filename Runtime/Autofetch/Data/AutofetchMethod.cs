using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace LnxArch
{
    public readonly struct AutofetchMethod
    {
        public MethodInfo Info { get; }
        public AutofetchAttribute AutofetchAttribute { get; }
        public AutofetchParameter[] Parameters { get; }
        public AutofetchType DeclaringType { get; }

        private readonly object[] _valuesBuffer;

        public AutofetchMethod(MethodInfo method, AutofetchAttribute autofetchAttribute,
            AutofetchParameter[] parameters, AutofetchType declaringType)
        {
            Info = method;
            AutofetchAttribute = autofetchAttribute;
            Parameters = parameters;
            DeclaringType = declaringType;
            _valuesBuffer = new object[parameters.Length];
        }

        public static AutofetchMethod BuildFrom(MethodInfo method, AutofetchType declaringType)
        {
            // TODO: Verify if method has generics and throw and exception if it does
            return new AutofetchMethod(
                method: method,
                autofetchAttribute: method.GetCustomAttribute<AutofetchAttribute>(false),
                parameters: method
                    .GetParameters()
                    .Select(param => AutofetchParameter.BuildFrom(param, declaringType))
                    .ToArray(),
                declaringType: declaringType
            );
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
                AutofetchParameter param = Parameters[i];
                List<Component> fetchedComponents = values[i];
                _valuesBuffer[i] = param.AdaptToBeValueOnInvokeParameter(fetchedComponents);
            }
            this.Info.Invoke(behaviour, _valuesBuffer);
        }
    }
}
