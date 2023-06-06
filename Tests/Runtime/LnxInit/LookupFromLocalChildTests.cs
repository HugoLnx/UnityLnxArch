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
    public class LookupFromLocalChildTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseLookupFromLocalChild Main { get; private set; }
            public BoxCollider ChildBoxCollider { get; private set; }
            public BoxCollider LocalBoxCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                var main = entity.FetchFirst<CaseLookupFromLocalChild>();
                return new EntityContext {
                    Main = main,
                    ChildBoxCollider = main.transform.GetChild(0).GetComponentInChildren<BoxCollider>(),
                    LocalBoxCollider = main.GetComponent<BoxCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void WhenFetchingOne_GetTheFirstComponentFoundInLocalChildren()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalChild_Default");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ChildBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.ChildBoxCollider));
        }

        [Test]
        public void WhenFetchingMany_GetAllTheComponentsFoundInLocalChildren()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalChild_Default");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ChildBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.ChildBoxCollider,
            }));
        }
        [Test]
        public void When_LocalHasMatch_FetchingOne_GetTheLocalComponent()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalChild_WithLocalCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.LocalBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.LocalBoxCollider));
        }

        [Test]
        public void When_LocalHasMatch_FetchingOne_IncludesTheLocalComponent()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocalChild_WithLocalCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.LocalBoxCollider, Is.Not.Null);
            Assert.That(ctx.ChildBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.LocalBoxCollider,
                ctx.ChildBoxCollider,
            }));
        }
    }
}
