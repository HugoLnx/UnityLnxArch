using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;

namespace LnxArch.LnxInitTests
{
    public class MethodWithoutParameters
    {
        #region Nested
        public struct EntityContext
        {
            public CaseMethodWithoutParameters Main { get; private set; }
            public CallsRegister Register { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main = entity.GetComponentInChildren<CaseMethodWithoutParameters>(),
                    Register = entity.GetComponentInChildren<CallsRegister>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void InitializationMethodShouldBeCalledAnyway()
        {
            var entity = _fixtures.InstantiateEntityPrefab("MethodWithoutParameters");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main, Is.Not.Null);

            Assert.IsTrue(ctx.Register.Contains(typeof(CaseMethodWithoutParameters), nameof(CaseMethodWithoutParameters.Prepare)));
        }
    }
}
