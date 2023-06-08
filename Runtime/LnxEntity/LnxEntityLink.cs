using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [DefaultExecutionOrder (-9997)]
    public class LnxEntityLink : MonoBehaviour
    {
        private static readonly ObjectLinkEvent[] s_whenEntityAffectsTarget = new ObjectLinkEvent[]
        {
            ObjectLinkEvent.Enable,
            ObjectLinkEvent.Disable,
            ObjectLinkEvent.Destroy
        };
        [SerializeField] public LnxEntity _entity;
        [SerializeField] private List<ObjectLinkEvent> _whenTargetAffectsEntity;
        [SerializeField] private bool _extendEntityFetching = true;
        private LnxObjectLinkService _service;

        private GameObject Target => gameObject;

        private void Awake()
        {
            _service = new LnxObjectLinkService(_entity.gameObject, Target);
            if (_extendEntityFetching && !_entity.Contains(this))
            {
                _entity.AddFetchExtension(Target);
            }
            EnforceLinks();
        }

        public void SetWhenTargetAffectsEntityAndEnforceLinks(List<ObjectLinkEvent> linkEvents)
        {
            _whenTargetAffectsEntity = linkEvents;
            EnforceLinks();
        }

        public void EnforceLinks()
        {
            EnforceEntityAffectsTargetOn(s_whenEntityAffectsTarget);
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
