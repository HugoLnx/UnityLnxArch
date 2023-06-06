using System.Collections.Generic;
using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseObjectsList : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public List<Collider> Colliders { get; private set; }
        public List<Rigidbody> Rigidbodies { get; private set; }

        [LnxInit]
        public void Prepare(List<Collider> colliders, List<Rigidbody> rigidbodies)
        {
            Colliders = colliders;
            Rigidbodies = rigidbodies;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] colliders:{Colliders.Count}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
