using System.Collections;
using System.Collections.Generic;
using LnxArch;
using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;
using UnityEngine.TestTools;

namespace LnxArch.LnxInitTests
{
    public class UsualCasePlayerTests
    {
        #region Nested
        public struct EntityContext
        {
            public Player Player { get; private set; }
            public Movement Movement { get; private set; }
            public Body Body { get; private set; }
            public Direction Direction { get; private set; }
            public Position Position { get; private set; }
            public Rotation Rotation { get; private set; }
            public CallsRegister Calls { get; private set; }
            public int PlayerCallInx => Calls.OrderOf(Player.GetType(), nameof(Player.Prepare));
            public int MovementCallInx => Calls.OrderOf(Movement.GetType(), nameof(Movement.Prepare));
            public int BodyCallInx => Calls.OrderOf(Body.GetType(), nameof(Body.Prepare));
            public int DirectionCallInx => Calls.OrderOf(Direction.GetType(), nameof(Direction.Prepare));
            public int PositionCallInx => Calls.OrderOf(Position.GetType(), nameof(Position.Prepare));
            public int RotationCallInx => Calls.OrderOf(Rotation.GetType(), nameof(Rotation.Prepare));

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Calls = entity.FetchFirst<CallsRegister>(),
                    Player = entity.FetchFirst<Player>(),
                    Movement = entity.FetchFirst<Movement>(),
                    Body = entity.FetchFirst<Body>(),
                    Direction = entity.FetchFirst<Direction>(),
                    Position = entity.FetchFirst<Position>(),
                    Rotation = entity.FetchFirst<Rotation>(),
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void DependenciesAreCalledFirst()
        {
            var entity = _fixtures.InstantiateEntityPrefab("UsualCasePlayer");
            var ctx = EntityContext.Create(entity);

            // All should be called
            Assert.That(ctx.PlayerCallInx, Is.GreaterThan(-1));
            Assert.That(ctx.MovementCallInx, Is.GreaterThan(-1));
            Assert.That(ctx.BodyCallInx, Is.GreaterThan(-1));
            Assert.That(ctx.DirectionCallInx, Is.GreaterThan(-1));
            Assert.That(ctx.PositionCallInx, Is.GreaterThan(-1));
            Assert.That(ctx.RotationCallInx, Is.GreaterThan(-1));

            // Player dependencies
            Assert.That(
                ctx.PlayerCallInx, Is.GreaterThan(ctx.MovementCallInx)
            );
            Assert.That(
                ctx.PlayerCallInx, Is.GreaterThan(ctx.DirectionCallInx)
            );
            Assert.That(
                ctx.PlayerCallInx, Is.GreaterThan(ctx.PositionCallInx)
            );
            Assert.That(
                ctx.PlayerCallInx, Is.GreaterThan(ctx.BodyCallInx)
            );

            // Movement dependencies
            Assert.That(
                ctx.MovementCallInx, Is.GreaterThan(ctx.DirectionCallInx)
            );
            Assert.That(
                ctx.MovementCallInx, Is.GreaterThan(ctx.PositionCallInx)
            );

            // Direction dependencies
            Assert.That(
                ctx.DirectionCallInx, Is.GreaterThan(ctx.RotationCallInx)
            );
        }

        [Test]
        public void DependenciesAreInjectedCorrectly()
        {
            var entity = _fixtures.InstantiateEntityPrefab("UsualCasePlayer");
            var ctx = EntityContext.Create(entity);

            // Player dependencies
            Assert.That(
                ctx.Player.Movement, Is.EqualTo(ctx.Movement)
            );
            Assert.That(
                ctx.Player.Direction, Is.EqualTo(ctx.Direction)
            );
            Assert.That(
                ctx.Player.Position, Is.EqualTo(ctx.Position)
            );
            Assert.That(
                ctx.Player.Body, Is.EqualTo(ctx.Body)
            );

            // Movement dependencies
            Assert.That(
                ctx.Movement.Direction, Is.EqualTo(ctx.Direction)
            );
            Assert.That(
                ctx.Movement.Position, Is.EqualTo(ctx.Position)
            );

            // Direction dependencies
            Assert.That(
                ctx.Direction.Rotation, Is.EqualTo(ctx.Rotation)
            );
        }
    }
}
