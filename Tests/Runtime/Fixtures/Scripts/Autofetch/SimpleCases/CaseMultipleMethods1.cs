using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseMultipleMethods1 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseMultipleMethods2 Case2 { get; private set; }
        public CaseMultipleMethods3 Case3 { get; private set; }
        public BoxCollider BoxCollider { get; private set; }
        public SphereCollider SphereCollider { get; private set; }

        [Autofetch]
        public void Prepare1(CaseMultipleMethods2 case2, BoxCollider boxCollider)
        {
            BoxCollider = boxCollider;
            Case2 = case2;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] boxCollider:{boxCollider} case2:{case2}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare1));
        }

        [Autofetch]
        public void Prepare2(CaseMultipleMethods3 case3, SphereCollider sphereCollider)
        {
            SphereCollider = sphereCollider;
            Case3 = case3;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] sphereCollider:{sphereCollider} case3:{case3}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare2));
        }
    }
}
