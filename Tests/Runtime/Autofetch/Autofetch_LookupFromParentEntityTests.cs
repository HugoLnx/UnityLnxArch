using System.Collections;
using System.Collections.Generic;
using LnxArch;
using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace LnxArch.Tests
{
    public class Autofetch_LookupFromParentEntityTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseLookupFromParentEntity Main { get; private set; }
            public BoxCollider BoxCollider { get; private set; }
            public SphereCollider SphereColliderInParent { get; private set; }
            public SphereCollider SphereColliderInChild { get; private set; }
            public CapsuleCollider CapsuleCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                var main = entity.GetComponentInChildren<CaseLookupFromParentEntity>();
                return new EntityContext {
                    Main = main,
                    BoxCollider = entity.GetComponentInChildren<BoxCollider>(),
                    SphereColliderInParent = main.transform.parent.GetComponentInParent<SphereCollider>(),
                    SphereColliderInChild = main.transform.GetChild(0).GetComponentInChildren<SphereCollider>(),
                    CapsuleCollider = entity.GetComponentInChildren<CapsuleCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;
        [Test]
        public void WhenFetchingOne_GetTheFirstComponentFoundInTheParentEntity()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromParentEntity");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.BoxCollider));
        }

        [Test]
        public void WhenFetchingMany_GetAllTheComponentsFoundInTheParentEntityAndItsChildEntities()
        // TODO: Change behaviour to only fetch from parent entity
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromParentEntity");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereColliderInParent, Is.Not.Null);
            Assert.That(ctx.SphereColliderInChild, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.BoxCollider,
                ctx.SphereColliderInParent,
                ctx.SphereColliderInChild,
                ctx.CapsuleCollider
            }));
        }
    }
}
