using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LnxArch
{
    public enum FetchCollectionWrap { None, Array, List }
    public class InitMethodParameter
    {
        public Type Type { get; }
        public Type ComponentType { get; }
        public InitType DeclaringType { get; set; }
        public InitMethod DeclaringMethod { get; set; }
        private FetchCollectionWrap CollectionWrap { get; }
        public ParameterInfo Info { get; }
        public IFetchAttribute[] FetchAttributes { get; }
        public AutoAddExecutor AutoAddExecutor { get; set; }

        public bool HasListWrapping => CollectionWrap == FetchCollectionWrap.List;
        public bool HasArrayWrapping => CollectionWrap == FetchCollectionWrap.Array;
        public bool HasValidCollectionWrap => !IsSingleValue;
        public bool IsSingleValue => CollectionWrap == FetchCollectionWrap.None;
        public bool HasAutoAddExecutor => AutoAddExecutor != null;

        public InitMethodParameter(Type type, Type componentType, FetchCollectionWrap collectionWrap,
        ParameterInfo parameter, IFetchAttribute[] fetchAttributes, InitType declaringType = null,
        InitMethod declaringMethod = null)
        {
            Type = type;
            ComponentType = componentType;
            DeclaringType = declaringType;
            DeclaringMethod = declaringMethod;
            CollectionWrap = collectionWrap;
            Info = parameter;
            FetchAttributes = fetchAttributes;
            if (typeof(IEnumerable<>).IsAssignableFrom(Type) && !HasValidCollectionWrap)
            {
                throw new InvalidCollectionParameterTypeException(this);
            }
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

        public string ToHumanName()
        {
            return $"{DeclaringMethod.ToHumanName()}({this.Type.Name} {this.Info.Name})";
        }
    }
}
