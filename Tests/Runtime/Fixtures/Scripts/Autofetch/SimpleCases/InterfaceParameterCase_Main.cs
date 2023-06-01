using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class InterfaceParameterCase_Main : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public IInterfaceParameterCase Implementer { get; private set; }

        [Autofetch]
        public void Prepare(IInterfaceParameterCase implementer)
        {
            Implementer = implementer;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] implementer:{Implementer}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
