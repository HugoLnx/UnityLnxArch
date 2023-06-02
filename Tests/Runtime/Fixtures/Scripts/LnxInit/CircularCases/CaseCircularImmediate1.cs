using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseCircularImmediate1 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseCircularImmediate0 Circular { get; private set; }

        [LnxInit]
        private void Prepare(CaseCircularImmediate0 circular)
        {
            Circular = circular;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] circular:{circular}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
