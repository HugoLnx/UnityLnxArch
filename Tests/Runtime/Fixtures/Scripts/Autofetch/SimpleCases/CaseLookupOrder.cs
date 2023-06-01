using System.Collections.Generic;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseLookupOrder : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Collider Collider { get; private set; }
        public List<Collider> Colliders { get; private set; }

        [Autofetch]
        public void PrepareOne(
            [FromLocal(LookupOrder = 0)]
            [FromLocalAncestor(LookupOrder = 1)]
            [FromLocalChild(LookupOrder = 2)]
            [FromEntity(LookupOrder = 3)]
            Collider collider = null
        )
        {
            Collider = collider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] collider:{Collider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareOne));
        }

        [Autofetch]
        public void PrepareMany(
            [FromLocal(LookupOrder = 0)]
            [FromLocalAncestor(LookupOrder = 1)]
            [FromLocalChild(LookupOrder = 2)]
            [FromEntity(LookupOrder = 3)]
            List<Collider> colliders
        )
        {
            Colliders = colliders;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] colliders:{Colliders.Count}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareMany));
        }
    }
}
