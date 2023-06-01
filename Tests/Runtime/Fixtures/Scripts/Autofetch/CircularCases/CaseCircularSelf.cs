using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseCircularSelf : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseCircularSelf Circular { get; private set; }

        [Autofetch]
        private void Prepare(CaseCircularSelf circular)
        {
            Circular = circular;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] circular:{circular}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
