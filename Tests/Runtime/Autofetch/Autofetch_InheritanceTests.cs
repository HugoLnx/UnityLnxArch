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
    public class Autofetch_InheritanceTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseInheritance_Grandfather Grandfather { get; private set; }
            public CaseInheritance_Father Father { get; private set; }
            public CaseInheritance_Son Son { get; private set; }
            public BoxCollider BoxCollider { get; private set; }
            public SphereCollider SphereCollider { get; private set; }
            public CapsuleCollider CapsuleCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Grandfather = entity.transform.Find("Grandfather").GetComponent<CaseInheritance_Grandfather>(),
                    Father = entity.transform.Find("Father").GetComponent<CaseInheritance_Father>(),
                    Son = entity.transform.Find("Son").GetComponent<CaseInheritance_Son>(),
                    BoxCollider = entity.FetchFirst<BoxCollider>(),
                    SphereCollider = entity.FetchFirst<SphereCollider>(),
                    CapsuleCollider = entity.FetchFirst<CapsuleCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;
        [Test]
        public void Calls_CallInheritedInitializationMethods()
        {
            var entity = _fixtures.InstantiateEntityPrefab("Inheritance");
            EntityContext ctx = EntityContext.Create(entity);

            Assert.That(ctx.Grandfather, Is.Not.Null);
            Assert.That(ctx.Father, Is.Not.Null);
            Assert.That(ctx.Son, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.Grandfather.BoxCollider, Is.EqualTo(ctx.BoxCollider));

            Assert.That(ctx.Father.BoxCollider, Is.EqualTo(ctx.BoxCollider));
            Assert.That(ctx.Father.SphereCollider, Is.EqualTo(ctx.SphereCollider));

            Assert.That(ctx.Son.BoxCollider, Is.EqualTo(ctx.BoxCollider));
            Assert.That(ctx.Son.SphereCollider, Is.EqualTo(ctx.SphereCollider));
            Assert.That(ctx.Son.CapsuleCollider, Is.EqualTo(ctx.CapsuleCollider));
        }

        [Test]
        public void LookupGrandfather_FetchAllComponentsThatInheritsGrandfather()
        {
            var entity = _fixtures.InstantiateEntityPrefab("Inheritance");
            EntityContext ctx = EntityContext.Create(entity);

            Assert.That(ctx.Grandfather, Is.Not.Null);
            Assert.That(ctx.Father, Is.Not.Null);
            Assert.That(ctx.Son, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.Son.Grandfathers, Is.EqualTo(new List<CaseInheritance_Grandfather> {
                ctx.Grandfather,
                ctx.Father,
                ctx.Son
            }));
            Assert.That(ctx.Son.Fathers, Is.EqualTo(new List<CaseInheritance_Father> {
                ctx.Father,
                ctx.Son
            }));
        }
    }
}
