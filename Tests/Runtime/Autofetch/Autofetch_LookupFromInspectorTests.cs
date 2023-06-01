using System.Collections.Generic;
using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;

namespace LnxArch.Tests
{
    public class Autofetch_LookupFromInspectorTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseLookupFromInspector_DefaultSerializableAttributes MainDefaultSerializableAttributes { get; private set; }
            public CaseLookupFromInspector_DefaultPublicAttributes MainDefaultPublicAttributes { get; private set; }
            public CaseLookupFromInspector_DefaultProperties MainDefaultProperties { get; private set; }
            public CaseLookupFromInspector_CustomAttr MainCustomAttr { get; private set; }
            public BoxCollider BoxCollider { get; private set; }
            public SphereCollider SphereCollider { get; private set; }
            public CapsuleCollider CapsuleCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    MainDefaultSerializableAttributes = entity.FetchFirst<CaseLookupFromInspector_DefaultSerializableAttributes>(),
                    MainDefaultPublicAttributes = entity.FetchFirst<CaseLookupFromInspector_DefaultPublicAttributes>(),
                    MainDefaultProperties = entity.FetchFirst<CaseLookupFromInspector_DefaultProperties>(),
                    MainCustomAttr = entity.FetchFirst<CaseLookupFromInspector_CustomAttr>(),
                    BoxCollider = entity.FetchFirst<BoxCollider>(),
                    SphereCollider = entity.FetchFirst<SphereCollider>(),
                    CapsuleCollider = entity.FetchFirst<CapsuleCollider>()
                };
            }
        }
        #endregion
        private const string PrefabName = "LookupFromInspector";
        private static readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;

        [Test]
        public void WhenFetchingOne_InjectedByDefaultSerializableAttribute()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainDefaultSerializableAttributes, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.MainDefaultSerializableAttributes.Collider, Is.EqualTo(ctx.BoxCollider));
        }

        [Test]
        public void WhenFetchingMany_InjectedByDefaultSerializableAttribute()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainDefaultSerializableAttributes, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.MainDefaultSerializableAttributes.Colliders, Is.EqualTo(new List<Collider> {
                ctx.BoxCollider,
                ctx.SphereCollider,
                ctx.CapsuleCollider,
            }));
        }

        [Test]
        public void WhenFetchingOne_InjectedByDefaultPublicAttribute()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainDefaultPublicAttributes, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.MainDefaultPublicAttributes.Collider, Is.EqualTo(ctx.BoxCollider));
        }

        [Test]
        public void WhenFetchingMany_InjectedByDefaultPublicAttribute()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainDefaultPublicAttributes, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.MainDefaultPublicAttributes.Colliders, Is.EqualTo(new List<Collider> {
                ctx.BoxCollider,
                ctx.SphereCollider,
                ctx.CapsuleCollider,
            }));
        }

        [Test]
        public void WhenFetchingOne_InjectedByDefaultProperties()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainDefaultProperties, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.MainDefaultProperties.Collider, Is.EqualTo(ctx.BoxCollider));
        }

        [Test]
        public void WhenFetchingMany_InjectedByDefaultProperties()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainDefaultProperties, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.MainDefaultProperties.Colliders, Is.EqualTo(new List<Collider> {
                ctx.BoxCollider,
                ctx.SphereCollider,
                ctx.CapsuleCollider,
            }));
        }

        [Test]
        public void WhenFetchingOne_InjectedByCustomAttr()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainCustomAttr, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);

            Assert.That(ctx.MainCustomAttr.Collider, Is.EqualTo(ctx.BoxCollider));
        }

        [Test]
        public void WhenFetchingMany_InjectedByCustomAttr()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.MainCustomAttr, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.MainCustomAttr.Colliders, Is.EqualTo(new List<Collider> {
                ctx.BoxCollider,
                ctx.SphereCollider,
                ctx.CapsuleCollider,
            }));
        }
    }
}
