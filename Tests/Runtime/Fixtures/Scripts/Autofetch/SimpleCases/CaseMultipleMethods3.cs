using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseMultipleMethods3 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public BoxCollider BoxCollider { get; private set; }
        public SphereCollider SphereCollider { get; private set; }
        public CapsuleCollider CapsuleCollider { get; private set; }

        [Autofetch]
        public void Prepare1(BoxCollider boxCollider)
        {
            BoxCollider = boxCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] boxCollider:{boxCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare1));
        }

        [Autofetch]
        public void Prepare2(SphereCollider sphereCollider)
        {
            SphereCollider = sphereCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] sphereCollider:{sphereCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare2));
        }

        [Autofetch]
        public void Prepare3(CapsuleCollider capsuleCollider)
        {
            CapsuleCollider = capsuleCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] capsuleCollider:{capsuleCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare3));
        }
    }
}
