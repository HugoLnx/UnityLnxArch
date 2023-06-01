using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseCircularSmall2 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseCircularSmall0 Circular { get; private set; }

        [Autofetch]
        private void Prepare(CaseCircularSmall0 circular)
        {
            Circular = circular;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] circular:{circular}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
