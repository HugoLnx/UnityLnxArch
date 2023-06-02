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
    public class LookupOrderTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseLookupOrder Main { get; private set; }
            public BoxCollider ParentBoxCollider { get; private set; }
            public BoxCollider LocalBoxCollider { get; private set; }
            public SphereCollider ChildCollider { get; private set; }
            public CapsuleCollider EntityCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                var main = entity.FetchFirst<CaseLookupOrder>();
                return new EntityContext {
                    Main = main,
                    ParentBoxCollider = main.transform.parent.GetComponent<BoxCollider>(),
                    LocalBoxCollider = main.GetComponent<BoxCollider>(),
                    ChildCollider = entity.FetchFirst<SphereCollider>(),
                    EntityCollider = entity.FetchFirst<CapsuleCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void WhenAllAttributesMatch_FetchingOne_GetMatchFromAttributeOfOrderZero()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_AllColliders");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.LocalBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.LocalBoxCollider));
        }
        [Test]
        public void WhenAllAttributesMatch_FetchingMany_GetMatchesFromAttributeOfOrderZero()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_AllColliders");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.LocalBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.LocalBoxCollider
            }));
        }
        [Test]
        public void WhenOrderZeroDoesntMatch_FetchingOne_GetMatchFromAttributeOfOrderOne()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoLocalCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ParentBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.ParentBoxCollider));
        }
        [Test]
        public void WhenOrderZeroDoesntMatch_FetchingMany_GetMatchesFromAttributeOfOrderOne()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoLocalCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ParentBoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.ParentBoxCollider
            }));
        }
        [Test]
        public void WhenOrderOneDoesntMatch_FetchingOne_GetMatchFromAttributeOfOrderTwo()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoAncestorCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ChildCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.ChildCollider));
        }
        [Test]
        public void WhenOrderOneDoesntMatch_FetchingMany_GetMatchesFromAttributeOfOrderTwo()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoAncestorCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.ChildCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.ChildCollider
            }));
        }
        [Test]
        public void WhenOrderTwoDoesntMatch_FetchingOne_GetMatchFromAttributeOfOrderThree()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoChildCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.EntityCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.EntityCollider));
        }
        [Test]
        public void WhenOrderTwoDoesntMatch_FetchingMany_GetMatchesFromAttributeOfOrderThree()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoChildCollider");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.EntityCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.EntityCollider
            }));
        }
        [Test]
        public void WhenDoesntMatchAny_FetchingOne_GetDefaultParameterIfAny()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoMatch");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.Null);
        }
        [Test]
        public void WhenDoesntMatchAny_FetchingMany_GetEmptyList()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupOrder_NoMatch");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.Empty);
        }
    }
}
