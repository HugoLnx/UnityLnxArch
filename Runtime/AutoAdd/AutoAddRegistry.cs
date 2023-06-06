using UnityEngine;
using System.Collections.Generic;
using System;

namespace LnxArch
{
    public enum AutoAddType
    {
        Local,
        Entity,
        Service
    }
    public class AutoAddRegistry
    {
        public static AutoAddRegistry Instance { get; } = new();
        private readonly Dictionary<Type, AutoAddExecutor> _executors = new();
        private AutoAddExecutor _localExecutor;
        private AutoAddExecutor _entityExecutor;
        private AutoAddExecutor _serviceExecutor;

        public AutoAddRegistry()
        {
            _localExecutor = new AutoAddExecutor(new OwnerLookupLocal(), new SimpleAutoAdder());
            _entityExecutor = new AutoAddExecutor(new OwnerLookupEntity(), new SimpleAutoAdder());
            _serviceExecutor = new AutoAddExecutor(new OwnerLookupService(), new ServiceAutoAdder());
        }

        public void Register(Type type, AutoAddType addType)
        {
            if (_executors.ContainsKey(type)) return;

            _executors.Add(type, ExecutorFromAddType(addType));
        }

        private AutoAddExecutor ExecutorFromAddType(AutoAddType addType)
        {
            return addType switch {
                AutoAddType.Local => _localExecutor,
                AutoAddType.Entity => _entityExecutor,
                AutoAddType.Service => _serviceExecutor,
                _ => throw new System.NotImplementedException()
            };
        }

        public AutoAddExecutor Retrieve<T>() where T : MonoBehaviour
        {
            return _executors.GetValueOrDefault(typeof(T));
        }

        public AutoAddExecutor Retrieve(Type type)
        {
            return _executors.GetValueOrDefault(type);
        }
    }
}
