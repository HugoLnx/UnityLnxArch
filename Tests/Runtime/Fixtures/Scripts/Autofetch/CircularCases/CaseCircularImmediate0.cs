using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseCircularImmediate0 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseCircularImmediate1 Circular { get; private set; }

        [Autofetch]
        private void Prepare(CaseCircularImmediate1 circular)
        {
            Circular = circular;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] circular:{circular}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
