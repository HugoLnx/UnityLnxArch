using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseCircularSmall1 : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseCircularSmall2 Circular { get; private set; }

        [Autofetch]
        private void Prepare(CaseCircularSmall2 circular)
        {
            Circular = circular;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] circular:{circular}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
