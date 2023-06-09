using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LnxArch.ComponentUtility;

namespace LnxArch
{
    public enum ReattachMoment
    {
        AtLnxInit,
        AfterAwake,
        AfterStart,
        AfterFiveSeconds
    }

    [DefaultExecutionOrder (ExecutionOrderConfig.LnxReattach)]
    public class LnxReattach : MonoBehaviour
    {
        [SerializeField] private Transform _destination;
        [SerializeField] private ReattachMoment _when;
        [SerializeField] private List<ObjectLinkEvent> _reflectOnEntity;
        private LnxEntity _entity;
        private LnxEntityLinkManual _entityLink;

        [LnxInit]
        private void Init()
        {
            _entity = LnxEntity.FetchEntityOf(this, canBeNull: false);
            if (_when == ReattachMoment.AtLnxInit)
            {
                DoReattach();
            }
        }

        private void Awake()
        {
            if (_when == ReattachMoment.AfterAwake)
            {
                DoReattach();
            }
        }

        private IEnumerator Start()
        {
            if (_when == ReattachMoment.AfterStart)
            {
                DoReattach();
            }
            else if (_when == ReattachMoment.AfterFiveSeconds)
            {
                yield return new WaitForSeconds(5);
                DoReattach();
            }
        }

        public void DoReattach()
        {
            MoveToDestination();
            LinkWithEntity();
        }

        private void MoveToDestination()
        {
            if (_destination == null)
            {
                transform.SetParent(null);
            }
            else
            {
                transform.SetParent(_destination);
            }
        }

        private void LinkWithEntity()
        {
            if (_entity.gameObject == this.gameObject) return;
            _entityLink = EnsureComponent<LnxEntityLinkManual>(gameObject);
            _entityLink.ManualAwake(
                entity: _entity,
                whenTargetAffectsEntity: _reflectOnEntity,
                extendEntityFetching: true
            );
        }
    }
}
