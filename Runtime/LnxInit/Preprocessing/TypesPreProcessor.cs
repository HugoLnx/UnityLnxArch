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

        private Dictionary<Type, InitType> MapInitType =>
            _mapInitType ??=
            AllTypes
            .Select(t => InitType.TryToBuildFrom(t))
            .Where(t => t != null)
            .ToDictionary(t => t.Type, t => t);

        private IEnumerable<Type> AllTypes =>
            _allTypes ??=
            AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes());

        private TypesPreProcessor() { }

        public bool IsInitType(Type type)
        {
            return MapInitType.ContainsKey(type);
        }

        public InitType GetInitTypeOf(Type type)
        {
            return MapInitType.GetValueOrDefault(type);
        }
    }
}
