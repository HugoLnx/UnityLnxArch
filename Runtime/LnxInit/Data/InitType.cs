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
    }
}
