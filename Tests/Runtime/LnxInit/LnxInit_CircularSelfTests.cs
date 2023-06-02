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
    public class LnxInit_CircularSelfTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseCircularSelf Main { get; private set; }
            public CallsRegister Register { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main = entity.FetchFirst<CaseCircularSelf>(),
                    Register = entity.FetchFirst<CallsRegister>(),
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void InjectsItSelf()
        {
            var entity = _fixtures.InstantiateEntityPrefab("CircularSelf");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Main.Circular, Is.EqualTo(ctx.Main));
        }

        [Test]
        public void OnlyInitializesOnce()
        {
            var entity = _fixtures.InstantiateEntityPrefab("CircularSelf");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Register.Calls.Count, Is.EqualTo(1));
        }
    }
}
