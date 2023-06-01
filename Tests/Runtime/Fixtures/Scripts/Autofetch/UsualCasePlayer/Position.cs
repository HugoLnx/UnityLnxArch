using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class Position : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Transform Transform;

        [Autofetch]
        public void Prepare(Transform trans)
        {
            Transform = trans;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] transform:{Transform}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
