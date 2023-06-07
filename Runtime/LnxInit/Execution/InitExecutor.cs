using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace LnxArch
{
    // TODO: Put traversing logic into interface to make easier to test
    // different implementations and optimizations
    // TODO: Optimize to generate minimum garbage
    // TODO: Optimize to get entity more effectively before visiting node
    // TODO: Reset initialized behaviours to avoid memory leaks
    public sealed class InitExecutor
    {
        public static InitExecutor Instance { get; } = new(TypesPreProcessor.Instance);

        private readonly TypesPreProcessor _typesPreProcessor;
        private readonly HashSet<MonoBehaviour> _initialized;

        private InitExecutor(TypesPreProcessor typesPreProcessor) {
            _typesPreProcessor = typesPreProcessor;
            _initialized = new HashSet<MonoBehaviour>();
        }

        public void ExecuteInitMethodsOn(MonoBehaviour behaviour, bool force = false)
        {
            Assert.IsNotNull(behaviour);
            if (_initialized.Contains(behaviour) && !force) return;

            InitType type = null;
            LnxEntity entity = null;
            GetInitTypeAndEntity(behaviour, ref type, ref entity);
            if (type == null) return;

            TraverseTreeInitializingDependenciesFirst(behaviour, type, entity);
        }

        private void TraverseTreeInitializingDependenciesFirst(Component component, InitType initType = null, LnxEntity entity = null)
        {
            MonoBehaviour behaviour = component as MonoBehaviour;
            if (behaviour == null || _initialized.Contains(behaviour)) return;

            GetInitTypeAndEntity(behaviour, ref initType, ref entity);
            if (initType == null) return;

            _initialized.Add(behaviour);
            foreach (InitMethod method in initType.InitMethods)
            {
                List<Component>[] values = FetchDependenciesOf(method, behaviour, entity);
                foreach (Component dependency in Flatten(values))
                {
                    TraverseTreeInitializingDependenciesFirst(dependency);
                }
                method.InvokeWithValues(behaviour, values);
            }
        }

        private void GetInitTypeAndEntity(MonoBehaviour behaviour, ref InitType type, ref LnxEntity entity)
        {
            type ??= _typesPreProcessor.GetInitTypeOf(behaviour.GetType());
            if (type == null) return;

            entity ??= LnxEntity.FetchEntityOf(behaviour);
            if (entity == null)
            {
                throw new BehaviourWithoutEntityException(behaviour);
            }
        }

        private List<Component>[] FetchDependenciesOf(InitMethod method, MonoBehaviour behaviour, LnxEntity entity)
        {
            List<Component>[] values = new List<Component>[method.Parameters.Length];
            for (int i = 0; i < method.Parameters.Length; i++)
            {
                InitMethodParameter param = method.Parameters[i];
                List<Component> fetched = FetchParameter(param, method, behaviour, entity);
                values[i] = fetched;
            }
            return values;
        }

        private List<Component> FetchParameter(InitMethodParameter param, InitMethod method, MonoBehaviour behaviour, LnxEntity entity)
        {
            List<Component> fetched = param.FetchAttributes.Any()
                ? TryFetchWithAttributesInOrder(param, behaviour, entity)
                : TryFetchWithDefaultAttribute(param, behaviour, entity);

            bool hasFetched = fetched?.Count > 0 && fetched[0] != null;
            if (param.HasAutoAddExecutor && !hasFetched)
            {
                fetched = new List<Component> { param.AutoAddExecutor.ExecuteAt(behaviour, param.Type) };
            }
            else if (param.IsSingleValue && !hasFetched && !param.Info.HasDefaultValue)
            {
                throw new LnxParameterNotFulfilledException(param);
            }
            return fetched;
        }

        private static List<Component> TryFetchWithAttributesInOrder(InitMethodParameter param, MonoBehaviour behaviour, LnxEntity entity)
        {
            foreach (IFetchAttribute fetchAttribute in param.FetchAttributes)
            {
                List<Component> fetched = FetchDependencyWith(fetchAttribute, param, behaviour, entity);
                if (fetched.Count > 0 && fetched[0] != null)
                {
                    return fetched;
                }
            }
            return new List<Component>();
        }

        private List<Component> TryFetchWithDefaultAttribute(InitMethodParameter param, MonoBehaviour behaviour, LnxEntity entity)
        {
            IFetchAttribute defaultFetchAttribute = _typesPreProcessor.IsServiceType(param.Type)
                ? new FromServiceRegistryAttribute()
                : new FromEntityAttribute();
            return FetchDependencyWith(defaultFetchAttribute, param, behaviour, entity);
        }

        private static List<Component> FetchDependencyWith(IFetchAttribute fetchAttribute, InitMethodParameter param, MonoBehaviour behaviour, LnxEntity entity)
        {
            if (param.HasValidCollectionWrap)
            {
                return fetchAttribute
                    .FetchMany(new FetchContext { Behaviour = behaviour, Entity = entity, Type = param.ComponentType })
                    .ToList();
            }
            else
            {
                return new List<Component> { fetchAttribute.FetchOne(new FetchContext { Behaviour = behaviour, Entity = entity, Type = param.ComponentType }) };
            }
        }

        private static void AssertNotNull<T, K>(object v)
        {
            Debug.Assert(v != null && !v.Equals(null), $"{typeof(T)}'s dependency {typeof(K)} received no value on construction.");
        }

        private static IEnumerable<Component> Flatten(IEnumerable<IEnumerable<Component>> componentLists)
        {
            foreach (IEnumerable<Component> componentList in componentLists)
            {
                foreach (Component component in componentList)
                {
                    yield return component;
                }
            }
        }
    }
}
