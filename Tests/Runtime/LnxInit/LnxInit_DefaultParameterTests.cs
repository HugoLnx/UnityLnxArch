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
    public class LnxInit_DefaultParameterTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseDefaultParameter Main { get; private set; }
            public Collider Collider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main = entity.FetchFirst<CaseDefaultParameter>(),
                    Collider = entity.FetchFirst<Collider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void WhenComponentIsFound_InjectsIt()
        {
            var entity = _fixtures.InstantiateEntityPrefab("DefaultParameter_Found");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.Collider, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.EqualTo(ctx.Collider));
        }
        [Test]
        public void WhenComponentIsNotFound_UsesDefaultValue()
        {
            var entity = _fixtures.InstantiateEntityPrefab("DefaultParameter_NotFound");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Main.Collider, Is.Null);
        }
    }
}
