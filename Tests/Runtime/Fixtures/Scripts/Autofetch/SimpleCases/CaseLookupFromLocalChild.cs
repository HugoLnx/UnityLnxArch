using System.Collections.Generic;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseLookupFromLocalChild : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Collider Collider { get; private set; }
        public List<Collider> Colliders { get; private set; }

        [Autofetch]
        public void PrepareOne(
            [FromLocalChild]
            Collider collider
        )
        {
            Collider = collider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] collider:{Collider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareOne));
        }

        [Autofetch]
        public void PrepareMany(
            [FromLocalChild]
            List<Collider> colliders
        )
        {
            Colliders = colliders;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] colliders:{Colliders.Count}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareMany));
        }
    }
}
