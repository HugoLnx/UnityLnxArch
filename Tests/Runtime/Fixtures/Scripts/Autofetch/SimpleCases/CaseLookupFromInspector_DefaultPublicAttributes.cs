using System.Collections.Generic;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class CaseLookupFromInspector_DefaultPublicAttributes : MonoBehaviour
    {
        public new Collider collider;
        public List<Collider> colliders;
        [SerializeField] private bool _log;
        public Collider Collider { get; private set; }
        public List<Collider> Colliders { get; private set; }

        [LnxInit]
        public void PrepareOne(
            [FromInspector]
            Collider collider
        )
        {
            Collider = collider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] collider:{Collider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareOne));
        }

        [LnxInit]
        public void PrepareMany(
            [FromInspector]
            List<Collider> colliders
        )
        {
            Colliders = colliders;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] colliders:{Colliders.Count}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareMany));
        }
    }
}
