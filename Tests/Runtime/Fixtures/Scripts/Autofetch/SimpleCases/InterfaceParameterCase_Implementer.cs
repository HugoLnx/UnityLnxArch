using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class InterfaceParameterCase_Implementer : MonoBehaviour, IInterfaceParameterCase
    {
        [SerializeField] private bool _log;
        public Collider Collider { get; private set; }

        [LnxInit]
        public void Prepare(Collider collider)
        {
            Collider = collider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] collider:{Collider}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
