using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class AbstractParameterCase_Main : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public AAbstractParameterCase Implementer { get; private set; }

        [Autofetch]
        public void Prepare(AAbstractParameterCase implementer)
        {
            Implementer = implementer;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] implementer:{Implementer}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
