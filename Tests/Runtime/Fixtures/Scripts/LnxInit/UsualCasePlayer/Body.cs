using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Rigidbody Rigidbody;

        [LnxInit]
        public void Prepare(Rigidbody rigidbody)
        {
            Rigidbody = rigidbody;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] rigidbody:{Rigidbody}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
