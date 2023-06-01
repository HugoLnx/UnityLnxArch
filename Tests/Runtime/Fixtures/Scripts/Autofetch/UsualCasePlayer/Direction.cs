using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class Direction : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Rotation Rotation;

        [Autofetch]
        public void Prepare(Rotation rotation)
        {
            Rotation = rotation;
           if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] rotation:{Rotation}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
