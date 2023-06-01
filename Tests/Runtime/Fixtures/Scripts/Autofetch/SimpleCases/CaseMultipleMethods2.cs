using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseMultipleMethods2 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public BoxCollider BoxCollider { get; private set; }
        public SphereCollider SphereCollider { get; private set; }

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
    }
}
