using System.Collections;
using System.Collections.Generic;
using LnxArch;
using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace LnxArch.LnxInitTests
{
    public class LookupFromLocalAncestorTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseLookupFromLocalAncestor Main { get; private set; }
            public BoxCollider ParentBoxCollider { get; private set; }
            public BoxCollider LocalBoxCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                var main = entity.FetchFirst<CaseLookupFromLocalAncestor>();
                return new EntityContext {
                    Main = main,
                    ParentBoxCollider = main.transform.parent.GetComponentInParent<BoxCollider>(),
                    LocalBoxCollider = main.GetComponent<BoxCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void WhenFetchingOne_GetTheFirstComponentFoundInTheLocalAncestor()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalAncestor_Default");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ParentBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.ParentBoxCollider));
        }

        [Test]
        public void WhenFetchingMany_GetAllTheComponentsFoundInTheLocalAncestors()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalAncestor_Default");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ParentBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.ParentBoxCollider,
            }));
        }
        [Test]
        public void When_LocalHasMatch_FetchingOne_GetTheLocalComponent()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalAncestor_WithLocalCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.LocalBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.LocalBoxCollider));
        }

        [Test]
        public void When_LocalHasMatch_FetchingOne_IncludesTheLocalComponent()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalAncestor_WithLocalCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.LocalBoxCollider, Is.Not.Null);
            Assert.That(ctx.ParentBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.LocalBoxCollider,
                ctx.ParentBoxCollider,
            }));
        }
    }
}
