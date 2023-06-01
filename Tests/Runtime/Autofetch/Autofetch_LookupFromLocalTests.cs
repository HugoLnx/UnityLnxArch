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
    public class Autofetch_LookupFromLocalTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseLookupFromLocal Main { get; private set; }
            public BoxCollider BoxCollider { get; private set; }
            public SphereCollider SphereCollider { get; private set; }
            public CapsuleCollider CapsuleCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main = entity.FetchFirst<CaseLookupFromLocal>(),
                    BoxCollider = entity.FetchFirst<BoxCollider>(),
                    SphereCollider = entity.FetchFirst<SphereCollider>(),
                    CapsuleCollider = entity.FetchFirst<CapsuleCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;
        [Test]
        public void WhenFetchingOne_GetTheFirstComponentFoundInTheLocalObject()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocal");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.BoxCollider));
        }

        [Test]
        public void WhenFetchingMany_GetAllTheComponentsFoundInTheEntity()
        {
            var entity = _fixtures.InstantiateEntityPrefab("LookupFromLocal");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.Main.Colliders, Is.EqualTo(new List<Collider> {
                ctx.BoxCollider,
            }));
        }
    }
}
