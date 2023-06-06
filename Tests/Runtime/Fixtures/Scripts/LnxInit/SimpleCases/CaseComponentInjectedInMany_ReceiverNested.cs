using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseComponentInjectedInMany_ReceiverNested : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseComponentInjectedInMany_Target Target { get; private set; }
        public CaseComponentInjectedInMany_Receiver Receiver { get; private set; }

        [LnxInit]
        public void Prepare(CaseComponentInjectedInMany_Target target, CaseComponentInjectedInMany_Receiver receiver)
        {
            Target = target;
            Receiver = receiver;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] target:{Target} receiver:{Receiver}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
