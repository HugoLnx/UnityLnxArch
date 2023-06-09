using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [DefaultExecutionOrder (ExecutionOrderConfig.LnxEntityLink)]
    public class LnxEntityLink : MonoBehaviour
    {
        public static readonly List<ObjectLinkEvent> AllEvents = new()
        {
            ObjectLinkEvent.Enable,
            ObjectLinkEvent.Disable,
            ObjectLinkEvent.Destroy
        };
        [SerializeField] protected LnxEntity _entity;
        [SerializeField] protected List<ObjectLinkEvent> _whenTargetAffectsEntity;
        [SerializeField] protected bool _extendEntityFetching = true;
        protected virtual bool IsManuallyAwaken => false;
        private LnxObjectLinkService _service;

        private GameObject Target => gameObject;

        protected virtual void Awake()
        {
            if (IsManuallyAwaken) return;
            Boot();
        }

        protected void Boot()
        {
            _service = new LnxObjectLinkService(_entity.gameObject, Target);
            if (_extendEntityFetching && !_entity.Contains(this))
            {
                _entity.AddFetchExtension(Target);
            }
            EnforceLinks();
        }

        public void EnforceLinks()
        {
            EnforceEntityAffectsTargetOn(AllEvents);
            EnforceTargetAffectsEntityOn(_whenTargetAffectsEntity);
        }

        private void EnforceEntityAffectsTargetOn(IEnumerable<ObjectLinkEvent> linkEvents)
        {
            foreach (ObjectLinkEvent linkEvent in linkEvents)
            {
                _service.Link(linkEvent, isAToB: true);
            }
        }

        private void EnforceTargetAffectsEntityOn(IEnumerable<ObjectLinkEvent> linkEvents)
        {
            foreach (ObjectLinkEvent linkEvent in linkEvents)
            {
                _service.Link(linkEvent, isAToB: false);
            }
        }
    }
}
