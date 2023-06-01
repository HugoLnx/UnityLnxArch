using System;
using System.Collections.Generic;
using System.Linq;

namespace LnxArch
{
    public sealed class TypesPreProcessor
    {
        public static TypesPreProcessor Instance { get; } = new TypesPreProcessor();
        private IEnumerable<Type> _allTypes;
        private Dictionary<Type, AutofetchType> _mapAutofetchType;

        private Dictionary<Type, AutofetchType> MapAutofetchType =>
            _mapAutofetchType ??=
            AllTypes
            .Select(t => AutofetchType.TryToBuildFrom(t))
            .Where(t => t != null)
            .ToDictionary(t => t.Type, t => t);

        private IEnumerable<Type> AllTypes =>
            _allTypes ??=
            AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes());

        private TypesPreProcessor() { }

        public bool IsAutofetchType(Type type)
        {
            return MapAutofetchType.ContainsKey(type);
        }

        public AutofetchType GetAutofetchTypeOf(Type type)
        {
            return MapAutofetchType.GetValueOrDefault(type);
        }
    }
}
