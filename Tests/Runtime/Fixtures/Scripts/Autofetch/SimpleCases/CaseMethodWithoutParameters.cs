using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseMethodWithoutParameters : MonoBehaviour
    {
        [SerializeField] private bool _log;

        [Autofetch]
        public void Prepare()
        {
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}]");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
