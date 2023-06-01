using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseInheritance_Father : CaseInheritance_Grandfather
    {
        public SphereCollider SphereCollider { get; private set; }

        [Autofetch]
        public void PrepareFather(SphereCollider sphereCollider)
        {
            SphereCollider = sphereCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] sphereCollider:{SphereCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareFather));
        }
    }
}
