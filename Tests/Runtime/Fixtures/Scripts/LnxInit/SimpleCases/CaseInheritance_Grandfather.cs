using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseInheritance_Grandfather : MonoBehaviour
    {
        [SerializeField] protected bool _log;
        public BoxCollider BoxCollider { get; private set; }

        [LnxInit]
        public void PrepareGrandfather(BoxCollider boxCollider)
        {
            BoxCollider = boxCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] boxCollider:{BoxCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareGrandfather));
        }
    }
}
