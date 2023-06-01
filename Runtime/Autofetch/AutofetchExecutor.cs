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
    // TODO: Reset autofetched behaviours to avoid memory leaks
    public sealed class AutofetchExecutor
    {
        public static AutofetchExecutor Instance { get; } = new(TypesPreProcessor.Instance);

        private readonly TypesPreProcessor _typesPreProcessor;
        private readonly HashSet<MonoBehaviour> _behavioursAutofetched;

        private AutofetchExecutor(TypesPreProcessor typesPreProcessor) {
            _typesPreProcessor = typesPreProcessor;
            _behavioursAutofetched = new HashSet<MonoBehaviour>();
        }

        public void ExecuteAutofetchOn(MonoBehaviour behaviour)
        {
            Assert.IsNotNull(behaviour);
            if (_behavioursAutofetched.Contains(behaviour))
            {
                return;
            }

            AutofetchType type = _typesPreProcessor.GetAutofetchTypeOf(behaviour.GetType());
            if (type == null)
            {
                return;
            }

            LnxEntity entity = LnxBehaviour.GetLnxEntity(behaviour);
            Assert.IsNotNull(entity, "Behaviours with Autofetch methods must be inside an LnxEntity");

            TraverseTreeAutofetchingDependenciesFirst(behaviour, type, entity);
        }

        private void TraverseTreeAutofetchingDependenciesFirst(Component component, AutofetchType type = null, LnxEntity entity = null)
        {
            MonoBehaviour behaviour = component as MonoBehaviour;
            if (behaviour == null || _behavioursAutofetched.Contains(behaviour))
            {
                return;
            }

            _behavioursAutofetched.Add(behaviour);
            type ??= _typesPreProcessor.GetAutofetchTypeOf(behaviour.GetType());
            entity ??= LnxBehaviour.GetLnxEntity(behaviour);
            foreach (AutofetchMethod method in type.AutofetchMethods)
            {
                List<Component>[] values = FetchDependenciesOf(method, behaviour, entity);
                foreach (Component dependency in Flatten(values))
                {
                    TraverseTreeAutofetchingDependenciesFirst(dependency);
                }
                method.InvokeWithValues(behaviour, values);
            }
        }

        private static List<Component>[] FetchDependenciesOf(AutofetchMethod method, MonoBehaviour behaviour, LnxEntity entity)
        {
            List<Component>[] values = new List<Component>[method.Parameters.Length];
            for (int i = 0; i < method.Parameters.Length; i++)
            {
                AutofetchParameter param = method.Parameters[i];
                List<Component> fetched = FetchParameter(param, method, behaviour, entity);
                values[i] = fetched;
            }
            return values;
        }

        private static List<Component> FetchParameter(AutofetchParameter param, AutofetchMethod method, MonoBehaviour behaviour, LnxEntity entity)
        {
            List<Component> fetched = null;
            foreach (IFetchAttribute fetchAttribute in param.FetchAttributes)
            {
                fetched = FetchDependencyWith(fetchAttribute, param, behaviour, entity);
                if (fetched.Count > 0 && fetched[0] != null) break;
            }

            // Debug.Log($"[Fetched:{behaviour.GetType().Name}] {param.Type.Name} {param.Info.Name} = {fetched?.GetType()}");
            if (!param.Info.HasDefaultValue)
            {
                string errorPrefix = $"[LnxArch:AutoFetch:{behaviour.GetType().Name}#{method.Info.Name}({param.Type.Name} {param.Info.Name})]";
                Assert.IsTrue(
                    !typeof(IEnumerable<>).IsAssignableFrom(param.Type)
                    || param.HasValidCollectionWrap,
                    $"{errorPrefix} Only types derived from Component, Component[] or List<Component> can be auto fetched.");
                Assert.IsNotNull(fetched,
                    $"{errorPrefix} Could not fulfill that dependency");
            }
            return fetched;
        }

        private static List<Component> FetchDependencyWith(IFetchAttribute fetchAttribute, AutofetchParameter param, MonoBehaviour behaviour, LnxEntity entity)
        {
            if (param.HasValidCollectionWrap)
            {
                return fetchAttribute
                    .FetchMany(behaviour, entity, param.ComponentType)
                    .ToList();
            }
            else
            {
                return new List<Component> { fetchAttribute.FetchOne(behaviour, entity, param.ComponentType) };
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
