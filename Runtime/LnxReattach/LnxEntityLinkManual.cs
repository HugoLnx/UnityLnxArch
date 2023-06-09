using System.Collections.Generic;
using UnityEngine;

namespace LnxArch
{
    [DefaultExecutionOrder (ExecutionOrderConfig.LnxEntityLink)]
    public class LnxEntityLinkManual : LnxEntityLink
    {
        protected override bool IsManuallyAwaken => true;

        public void ManualAwake(LnxEntity entity, List<ObjectLinkEvent> whenTargetAffectsEntity, bool extendEntityFetching)
        {
            _entity = entity;
            _whenTargetAffectsEntity = whenTargetAffectsEntity;
            _extendEntityFetching = extendEntityFetching;
            Boot();
        }
    }
}
