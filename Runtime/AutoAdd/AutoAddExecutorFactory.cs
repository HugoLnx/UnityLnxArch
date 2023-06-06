using UnityEngine;
using System.Collections.Generic;
using System;

namespace LnxArch
{
    public enum AutoAddTarget
    {
        Local,
        Entity,
        Service
    }
    public class AutoAddExecutorFactory
    {
        private readonly AutoAddExecutor _localExecutor;
        private readonly AutoAddExecutor _entityExecutor;
        private readonly AutoAddExecutor _serviceExecutor;

        public AutoAddExecutorFactory()
        {
            _localExecutor = new AutoAddExecutor(new OwnerLookupLocal(), new SimpleAutoAdder());
            _entityExecutor = new AutoAddExecutor(new OwnerLookupEntity(), new SimpleAutoAdder());
            _serviceExecutor = new AutoAddExecutor(new OwnerLookupService(), new ServiceAutoAdder());
        }

        public AutoAddExecutor ExecutorFor(AutoAddTarget target)
        {
            return target switch {
                AutoAddTarget.Local => _localExecutor,
                AutoAddTarget.Entity => _entityExecutor,
                AutoAddTarget.Service => _serviceExecutor,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}
