using System.Linq;
using System.Reflection;

namespace LnxArch
{
    public class InitMethodFactory
    {
        private readonly InitMethodParameterFactory _parameterFactory;

        public InitMethodFactory(InitMethodParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public InitMethod BuildFrom(MethodInfo method)
        {
            LnxInitAttribute initAttribute = method.GetCustomAttribute<LnxInitAttribute>(false);
            if (initAttribute == null) return null;
            InitMethod initMethod = new(
                method: method,
                initAttribute: initAttribute,
                parameters: method
                    .GetParameters()
                    .Select(param => _parameterFactory.BuildFrom(param))
                    .ToArray()
            );

            if (method.IsGenericMethod)
            {
                throw new GenericInitMethodException(initMethod);
            }
            return initMethod;
        }

        public void LinkHierarchyBack(InitMethod initMethod, InitType declaringType)
        {
            initMethod.DeclaringType = declaringType;
            foreach (InitMethodParameter initParam in initMethod.Parameters)
            {
                _parameterFactory.LinkHierarchyBack(initParam, declaringType, initMethod);
            }
        }
    }
}
