using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseComponentInjectedInMany_Receiver : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public CaseComponentInjectedInMany_Target Target { get; private set; }

        [Autofetch]
        public void Prepare(CaseComponentInjectedInMany_Target target)
        {
            Target = target;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] target:{Target}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
