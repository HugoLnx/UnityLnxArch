using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public class AutofetchType
    {
        public static Type BaseType = typeof(MonoBehaviour);
        public Type Type { get; private set; }
        public AutofetchMethod[] AutofetchMethods { get; private set; }
        public AutofetchAttribute MainAttribute { get; private set; }
        public Dictionary<string, FieldInfo> InspectorVisibleFields { get; private set; }
        public int Priority { get; set; }

        public AutofetchType(Type type, AutofetchMethod[] autofetchMethods,
            AutofetchAttribute mainAttribute, Dictionary<string, FieldInfo> inspectorVisibleFields)
        {
            Type = type;
            AutofetchMethods = autofetchMethods;
            MainAttribute = mainAttribute;
            InspectorVisibleFields = inspectorVisibleFields;
        }

        public static AutofetchType TryToBuildFrom(Type type)
        {
            if (!IsSupported(type)) return null;
            AutofetchType autofetchType =  new(
                type: type,
                autofetchMethods: null,
                mainAttribute: null,
                inspectorVisibleFields: GetInpectorVisibleFieldsOf(type)
            );
            IEnumerable<AutofetchMethod> autofetchMethods = BuildAutofetchMethodsOf(type, autofetchType);
            if (!autofetchMethods.Any()) return null;
            autofetchType.AutofetchMethods = autofetchMethods.ToArray();
            autofetchType.MainAttribute = autofetchMethods.First().AutofetchAttribute;
            return autofetchType;
        }

        private static Dictionary<string, FieldInfo> GetInpectorVisibleFieldsOf(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null)
                    .ToDictionary(f => f.Name);
        }

        public static bool IsSupported(Type type)
        {
            return type.IsClass
            && !type.IsAbstract
            && BaseType.IsAssignableFrom(type);
        }

        public IEnumerable<Type> FindTypeDependencies()
        {
            return AutofetchMethods
                .SelectMany(m => m.FindTypeDependencies())
                .Distinct();
        }

        public override string ToString()
        {
            return $"AutofetchType-{Type} {Priority}";
        }

        private static IEnumerable<AutofetchMethod> BuildAutofetchMethodsOf(Type type, AutofetchType declaringType)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Select(method => AutofetchMethod.BuildFrom(method, declaringType))
            .Where(m => m.AutofetchAttribute != null);
        }
    }
}
