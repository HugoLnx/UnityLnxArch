using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public class InitTypeFactory
    {
        private readonly InitMethodFactory _methodFactory;

        public InitTypeFactory(InitMethodFactory methodFactory)
        {
            _methodFactory = methodFactory;
        }

        public static InitTypeFactory BuildFactoriesHierarchy()
        {
            InitMethodParameterFactory initParamFactory = new();
            InitMethodFactory initMethodFactory = new(initParamFactory);
            return new(initMethodFactory);
        }

        public InitType TryToBuildFrom(Type type)
        {
            if (!InitType.IsSupported(type)) return null;
            InitType initType =  new(
                type: type,
                initMethods: BuildInitMethodsOf(type).ToArray(),
                inspectorVisibleFields: GetInpectorVisibleFieldsOf(type)
            );
            if (initType.InitMethods.Length == 0) return null;
            LinkHierarchyBack(initType);
            return initType;
        }

        private void LinkHierarchyBack(InitType initType)
        {
            foreach (InitMethod method in initType.InitMethods)
            {
                _methodFactory.LinkHierarchyBack(method, initType);
            }
        }

        private IEnumerable<InitMethod> BuildInitMethodsOf(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Select(method => _methodFactory.BuildFrom(method))
            .Where(m => m?.InitAttribute != null);
        }

        private static Dictionary<string, FieldInfo> GetInpectorVisibleFieldsOf(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null)
                    .ToDictionary(f => f.Name);
        }
    }
}
