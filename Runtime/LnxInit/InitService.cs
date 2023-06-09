using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LnxArch
{
    public sealed class InitService
    {
        public static InitService Instance { get; } = new(InitExecutor.Instance);
        private readonly InitExecutor _executor;
        private InitService(InitExecutor executor)
        {
            this._executor = executor;
        }

        public void InitBehaviour(MonoBehaviour behaviour, bool force = false)
        {
            _executor.ExecuteInitMethodsOn(behaviour, force);
        }

        public void InitEntity(LnxEntity entity)
        {
            InitChildComponents(entity);
            SetChildEntitiesAsInitialized(entity);
        }

        private void InitChildComponents(LnxEntity entity)
        {
            foreach (MonoBehaviour behaviour in entity.FetchAll<MonoBehaviour>(includeInactive: true))
            {
                InitBehaviour(behaviour);
            }
        }

        private void SetChildEntitiesAsInitialized(LnxEntity entity)
        {
            IEnumerable<LnxEntity> childEntities = entity.FetchAll<LnxEntity>(includeInactive: true);
            foreach (LnxEntity childEntity in childEntities)
            {
                childEntity.WasInitialized = true;
            }
        }
    }
}
