using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public abstract class AAbstractParameterCase : MonoBehaviour
    {
        [SerializeField] protected bool _log;
        public Collider BoxCollider { get; private set; }
        public abstract SphereCollider SphereCollider { get; protected set; }

        [LnxInit]
        public void PrepareFromAbstract(BoxCollider boxCollider)
        {
            BoxCollider = boxCollider;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] boxCollider:{BoxCollider}");
            CallsRegister.FindAndRecordFor(this, nameof(PrepareFromAbstract));
        }
    }
}
