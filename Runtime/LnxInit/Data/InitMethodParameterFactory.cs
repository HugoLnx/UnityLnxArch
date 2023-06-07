using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LnxArch
{
    public class InitMethodParameterFactory
    {
        public InitMethodParameter BuildFrom(ParameterInfo parameter)
        {
            FetchCollectionWrap collectionWrap = GetCollectionType(parameter.ParameterType);
            return new(
                type: parameter.ParameterType,
                componentType: GetInnerTypeFrom(parameter.ParameterType, collectionWrap),
                collectionWrap: collectionWrap,
                parameter: parameter,
                fetchAttributes: GetFetchAttributesOf(parameter)
            );
        }

        public void LinkHierarchyBack(InitMethodParameter initParam, InitType declaringType, InitMethod declaringMethod)
        {
            initParam.DeclaringMethod = declaringMethod;
            initParam.DeclaringType = declaringType;
            FillInspectorGettersOnFetchAttributes(initParam);
        }

        private Type GetInnerTypeFrom(Type type, FetchCollectionWrap wrap)
        {
            return wrap switch
            {
                FetchCollectionWrap.Array => type.GetElementType(),
                FetchCollectionWrap.List => type.GetGenericArguments()[0],
                _ => type
            };
        }

        private FetchCollectionWrap GetCollectionType(Type type)
        {
            if (type.IsArray)
            {
                return FetchCollectionWrap.Array;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return FetchCollectionWrap.List;
            }
            return FetchCollectionWrap.None;
        }

        private IFetchAttribute[] GetFetchAttributesOf(ParameterInfo parameter)
        {
            IEnumerable<IFetchAttribute> explicitDeclaredAttributes = Attribute.GetCustomAttributes(parameter)
                .Where(attr => typeof(IFetchAttribute).IsAssignableFrom(attr.GetType()))
                .Select(attr => (IFetchAttribute) attr)
                .OrderBy(attr => attr.LookupOrder);

            if (explicitDeclaredAttributes.Any())
            {
                return explicitDeclaredAttributes.ToArray();
            }
            else
            {
                return Array.Empty<IFetchAttribute>();
            }
        }

        private void FillInspectorGettersOnFetchAttributes(InitMethodParameter initParam)
        {
            foreach (IFetchAttribute attr in initParam.FetchAttributes)
            {
                FromInspectorAttribute fromInspectorAttribute = attr as FromInspectorAttribute;
                if (fromInspectorAttribute == null) continue;
                IInspectorValueGetter inspectorGetter = InspectorValueGetterFactory.TryBuildFor(
                    declaringType: initParam.DeclaringType,
                    lookupBaseName: fromInspectorAttribute.Attr ?? initParam.Info.Name
                );
                if (inspectorGetter == null)
                {
                    throw new CouldNotFindAttributeOrPropertyForFromInspectorException(
                        declaringType: initParam.DeclaringType,
                        parameter: initParam.Info,
                        attribute: fromInspectorAttribute
                    );
                }
                fromInspectorAttribute.InspectorGetter = inspectorGetter;
            }
        }
    }
}
