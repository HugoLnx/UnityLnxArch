using System.Collections;
using System.Collections.Generic;
using LnxArch;
using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;
using UnityEngine.TestTools;

namespace LnxArch.Tests
{
    public class LnxInit_AbstractParameterTests
    {

        #region Nested
        public struct EntityContext
        {
            public CallsRegister Calls { get; private set; }
            public AbstractParameterCase_Main Main { get; private set; }
            public AbstractParameterCase_Implementer Implementer { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Calls = entity.FetchFirst<CallsRegister>(),
                    Main = entity.FetchFirst<AbstractParameterCase_Main>(),
                    Implementer = entity.FetchFirst<AbstractParameterCase_Implementer>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void GetDependencyByAbstractClass()
        {
            var entity = _fixtures.InstantiateEntityPrefab("AbstractParameter");
            EntityContext ctx = EntityContext.Create(entity);

            Assert.That(ctx.Main, Is.Not.Null);
            Assert.That(ctx.Implementer, Is.Not.Null);

            Assert.That(ctx.Main.Implementer, Is.EqualTo(ctx.Implementer));
        }
    }
}
