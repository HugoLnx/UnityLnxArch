using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public enum FetchCollectionWrap { None, Array, List }
    public readonly struct AutofetchParameter
    {
        public Type Type { get; }
        public Type ComponentType { get; }
        public AutofetchType DeclaringType { get; }
        private FetchCollectionWrap CollectionWrap { get; }
        public ParameterInfo Info { get; }
        public IFetchAttribute[] FetchAttributes { get; }

        public bool HasListWrapping => CollectionWrap == FetchCollectionWrap.List;
        public bool HasArrayWrapping => CollectionWrap == FetchCollectionWrap.Array;
        public bool HasValidCollectionWrap => CollectionWrap != FetchCollectionWrap.None;

        public AutofetchParameter(Type type, Type componentType,
            FetchCollectionWrap collectionWrap, ParameterInfo parameter,
            IFetchAttribute[] fetchAttributes, AutofetchType declaringType)
        {
            Type = type;
            ComponentType = componentType;
            DeclaringType = declaringType;
            CollectionWrap = collectionWrap;
            Info = parameter;
            FetchAttributes = fetchAttributes;
            FillInspectorGetters();
        }

        public static AutofetchParameter BuildFrom(ParameterInfo parameter, AutofetchType declaringType)
        {
            FetchCollectionWrap collectionWrap = GetCollectionType(parameter.ParameterType);
            return new AutofetchParameter(
                type: parameter.ParameterType,
                componentType: GetInnerTypeFrom(parameter.ParameterType, collectionWrap),
                collectionWrap: collectionWrap,
                parameter: parameter,
                fetchAttributes: GetFetchAttributesOf(parameter),
                declaringType: declaringType
            );
        }

        public object AdaptToBeValueOnInvokeParameter(List<Component> components)
        {
            if (components == null) return null;

            if (HasArrayWrapping)
            {
                return CollectionParameterCaster.ForceCastToArray(
                    ComponentType,
                    components
                );
            }
            else if (HasListWrapping)
            {
                return CollectionParameterCaster.ForceCastToList(
                    ComponentType,
                    components
                );
            }
            else if (components.Count > 0)
            {
                return components[0];
            }
            else
            {
                return null;
            }
        }

        private void FillInspectorGetters()
        {
            foreach (IFetchAttribute attr in FetchAttributes)
            {
                FromInspectorAttribute fromInspectorAttribute = attr as FromInspectorAttribute;
                if (fromInspectorAttribute == null) continue;
                IInspectorValueGetter inspectorGetter = InspectorValueGetterFactory.TryBuildFor(
                    declaringType: this.DeclaringType,
                    lookupBaseName: fromInspectorAttribute.Attr ?? this.Info.Name
                );
                if (inspectorGetter == null)
                {
                    throw new CouldNotFindAttributeOrPropertyForFromInspector(
                        declaringType: this.DeclaringType,
                        parameter: this.Info,
                        attribute: fromInspectorAttribute
                    );
                }
                fromInspectorAttribute.InspectorGetter = inspectorGetter;
            }
        }

        private static Type GetInnerTypeFrom(Type type, FetchCollectionWrap wrap)
        {
            return wrap switch
            {
                FetchCollectionWrap.Array => type.GetElementType(),
                FetchCollectionWrap.List => type.GetGenericArguments()[0],
                _ => type
            };
        }

        private static FetchCollectionWrap GetCollectionType(Type type)
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

        private static IFetchAttribute[] GetFetchAttributesOf(ParameterInfo parameter)
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
                return new IFetchAttribute[] { new FromEntityAttribute() };
            }
        }
    }
}
