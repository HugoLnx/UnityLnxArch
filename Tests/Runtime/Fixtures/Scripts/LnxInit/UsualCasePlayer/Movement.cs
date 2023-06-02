using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Direction Direction;
        public Position Position;

        [LnxInit]
        public void Prepare(Direction direction, Position position)
        {
            Direction = direction;
            Position = position;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}-{this.name}] direction:{direction} position:{position}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }

    }
}
