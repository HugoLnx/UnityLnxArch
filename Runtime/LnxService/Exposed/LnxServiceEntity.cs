using UnityEngine;
using static LnxArch.ComponentUtility;

namespace LnxArch
{
    [DefaultExecutionOrder (ExecutionOrderConfig.LnxServiceEntity)]
    public class LnxServiceEntity : MonoBehaviour
    {
        private static readonly TypesPreProcessor s_types = TypesPreProcessor.Instance;
        private static readonly ServiceEntityRegistry s_registry = ServiceEntityRegistry.Instance;
        private bool _wasRegistered;
        private LnxEntity _entity;

        public bool IsPersistent { get; private set; }

        private void Awake()
        {
            if (_wasRegistered) return;
            LnxServiceEntity[] allServiceEntities = FindObjectsOfType<LnxServiceEntity>(includeInactive: true);
            foreach (LnxServiceEntity serviceEntity in allServiceEntities)
            {
                serviceEntity.ValidateNestedServiceEntities();
                serviceEntity.RegisterAllChildServices();
            }
            foreach (LnxServiceEntity serviceEntity in allServiceEntities)
            {
                serviceEntity.EnsureEntity();
            }
            foreach (LnxServiceEntity serviceEntity in allServiceEntities)
            {
                serviceEntity.AwakeEntity();
            }
        }

        private void RegisterAllChildServices()
        {
            MonoBehaviour persistentService = null;
            MonoBehaviour transientService = null;
            foreach (MonoBehaviour service in GetComponentsInChildren<MonoBehaviour>())
            {
                LnxServiceType serviceType = s_types.GetServiceType(service.GetType());
                if (serviceType == null) continue;
                else if (serviceType.IsPersistent) persistentService = service;
                else transientService = service;
                RegisterService(service);
            }
            _wasRegistered = true;
            if (persistentService != null && transientService != null)
            {
                throw new LnxServiceEntityInvalidChildrenException(this, transientService, persistentService);
            }
            else if (persistentService != null)
            {
                EnforcePersistent();
            }
        }

        private void RegisterService(MonoBehaviour behaviour)
        {
            s_registry.Register(behaviour);

            OnDisableObservable observeDisable = EnsureComponent<OnDisableObservable>(behaviour.gameObject);
            observeDisable.Callbacks += () => s_registry.Unregister(behaviour);

            OnEnableObservable observeEnable = EnsureComponent<OnEnableObservable>(behaviour.gameObject);
            observeEnable.Callbacks += () => s_registry.Register(behaviour);
        }

        private LnxEntity EnsureEntity()
        {
            var preventAwake = EnsureComponent<LnxEntityPreventAutomaticAwake>(gameObject);
            _entity = EnsureComponent<LnxEntity>(gameObject);
            Destroy(preventAwake);

            return _entity;
        }

        private void EnforcePersistent()
        {
            gameObject.transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            IsPersistent = true;
        }

        private void AwakeEntity()
        {
            _entity.ForceAwake();
        }

        private void ValidateNestedServiceEntities()
        {
            foreach (MonoBehaviour serviceEntity in GetComponentsInParent<LnxServiceEntity>())
            {
                if (serviceEntity != this)
                {
                    throw new LnxServiceEntityNestedException(parent: serviceEntity, child: this);
                }
            }
        }
    }
}
