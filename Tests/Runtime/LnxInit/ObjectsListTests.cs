using System.Collections.Generic;
using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;

namespace LnxArch.LnxInitTests
{
    public class ObjectsListTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseObjectsList Main { get; private set; }
            public BoxCollider BoxCollider { get; private set; }
            public SphereCollider SphereCollider { get; private set; }
            public CapsuleCollider CapsuleCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main = entity.FetchFirst<CaseObjectsList>(),
                    BoxCollider = entity.FetchFirst<BoxCollider>(),
                    SphereCollider = entity.FetchFirst<SphereCollider>(),
                    CapsuleCollider = entity.FetchFirst<CapsuleCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void WhenHasMatch_ReturnsAllMatchingComponents()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ObjectsList");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider>() {
                ctx.BoxCollider,
                ctx.SphereCollider,
                ctx.CapsuleCollider
            }));
        }

        [Test]
        public void WhenHasNoMatch_ReturnsEmptyArray()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ObjectsList");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Main.Rigidbodies, Is.Empty);
        }
    }
}