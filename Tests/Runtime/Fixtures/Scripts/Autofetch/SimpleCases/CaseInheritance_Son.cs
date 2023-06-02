using System.Collections.Generic;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseInheritance_Son : CaseInheritance_Father
    {
        public List<CaseInheritance_Grandfather> Grandfathers { get; private set; }
        public List<CaseInheritance_Father> Fathers { get; private set; }
        public CapsuleCollider CapsuleCollider { get; private set; }

        [LnxInit]
        public void PrepareSon(CapsuleCollider collider, List<CaseInheritance_Grandfather> grandfathers, List<CaseInheritance_Father> fathers)
        {
            Grandfathers = grandfathers;
            Fathers = fathers;
            CapsuleCollider = collider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] wheelCollider:{CapsuleCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareSon));
        }
    }
}
