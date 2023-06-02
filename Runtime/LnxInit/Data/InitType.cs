using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public class InitType
    {
        public static Type BaseType = typeof(MonoBehaviour);
        public Type Type { get; private set; }
        public InitMethod[] InitMethods { get; private set; }
        public Dictionary<string, FieldInfo> InspectorVisibleFields { get; private set; }
        public int Priority { get; set; }

        public InitType(Type type, InitMethod[] initMethods,
            Dictionary<string, FieldInfo> inspectorVisibleFields)
        {
            Type = type;
            InitMethods = initMethods;
            InspectorVisibleFields = inspectorVisibleFields;
        }

        public static InitType TryToBuildFrom(Type type)
        {
            if (!IsSupported(type)) return null;
            InitType initType =  new(
                type: type,
                initMethods: null,
                inspectorVisibleFields: GetInpectorVisibleFieldsOf(type)
            );
            IEnumerable<InitMethod> initMethods = BuildInitMethodsOf(type, initType);
            if (!initMethods.Any()) return null;
            initType.InitMethods = initMethods.ToArray();
            return initType;
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
            return InitMethods
                .SelectMany(m => m.FindTypeDependencies())
                .Distinct();
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}-{Type} {Priority}";
        }

        private static IEnumerable<InitMethod> BuildInitMethodsOf(Type type, InitType declaringType)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Select(method => InitMethod.BuildFrom(method, declaringType))
            .Where(m => m.InitAttribute != null);
        }
    }
}
