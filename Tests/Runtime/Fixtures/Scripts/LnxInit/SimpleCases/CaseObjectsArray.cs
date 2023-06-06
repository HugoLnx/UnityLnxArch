using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseObjectsArray : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Collider[] Colliders { get; private set; }
        public Rigidbody[] Rigidbodies { get; private set; }

        [LnxInit]
        public void Prepare(Collider[] colliders, Rigidbody[] rigidbodies)
        {
            Colliders = colliders;
            Rigidbodies = rigidbodies;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] colliders:{Colliders.Length}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
