using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;

namespace LnxArch.Tests
{
    public class Autofetch_InterfaceParameterTests
    {
        #region Nested
        public struct EntityContext
        {
            public InterfaceParameterCase_Main Main { get; private set; }
            public InterfaceParameterCase_Implementer Implementer { get; private set; }
            public BoxCollider BoxCollider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main = entity.FetchFirst<InterfaceParameterCase_Main>(),
                    Implementer = entity.FetchFirst<InterfaceParameterCase_Implementer>(),
                    BoxCollider = entity.FetchFirst<BoxCollider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;
        [Test]
        public void GetComponentByInterface()
        {
            var entity = _fixtures.InstantiateEntityPrefab("InterfaceParameter");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Implementer, Is.Not.Null);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.That(ctx.Main.Implementer, Is.EqualTo(ctx.Implementer));
        }
    }
}
