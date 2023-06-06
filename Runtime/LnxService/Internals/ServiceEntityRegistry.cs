using System;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    public sealed class ServiceEntityRegistry
    {
        public static ServiceEntityRegistry Instance { get; } = new();
        private readonly Dictionary<Type, MonoBehaviour> _services = new();
        private ServiceEntityRegistry() { }

        public void Register(MonoBehaviour behaviour)
        {
            Type type = behaviour.GetType();
            if (_services.ContainsKey(type)) return;

            _services.Add(type, behaviour);
        }

        public void Unregister(MonoBehaviour behaviour)
        {
            Type type = behaviour.GetType();
            if (!_services.ContainsKey(type)) return;
            _services.Remove(type);
        }

        public T Retrieve<T>() where T : MonoBehaviour
        {
            return (T) _services.GetValueOrDefault(typeof(T));
        }

        public MonoBehaviour Retrieve(Type type)
        {
            return _services.GetValueOrDefault(type);
        }
    }
}
