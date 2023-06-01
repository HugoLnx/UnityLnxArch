using System.Collections;
using LnxArch;
using LnxArch.TestTools;
using UnityEngine;

namespace LnxArch.TestFixtures
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private bool _log;
        public Movement Movement;
        public Direction Direction;
        public Position Position;
        public Body Body;

        [Autofetch]
        public void Prepare(Movement movement, Direction direction, Position position, Body body)
        {
            Movement = movement;
            Direction = direction;
            Position = position;
            Body = body;
            if (_log) Debug.Log($"[Prepared:{this.GetType().Name}] movement:{movement} direction:{direction} position:{position} body:{body}");
            CallsRegister.FindAndRecordFor(this, nameof(Prepare));
        }
    }
}
