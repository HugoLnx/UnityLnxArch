using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class AbstractParameterCase_Implementer : AAbstractParameterCase
    {
        public override SphereCollider SphereCollider { get; protected set; }

        [Autofetch]
        public void Prepare(SphereCollider sphereCollider)
        {
            SphereCollider = sphereCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] sphereCollider:{SphereCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
