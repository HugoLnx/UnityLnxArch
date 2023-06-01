using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseComponentInjectedInMany_Target : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Collider Collider { get; private set; }

        [Autofetch]
        public void Prepare(Collider collider = null)
        {
            Collider = collider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] collider:{Collider}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
