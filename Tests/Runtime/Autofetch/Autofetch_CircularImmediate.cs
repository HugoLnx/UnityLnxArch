using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;

namespace LnxArch.Tests
{
    public class Autofetch_CircularImmediate
    {
        #region Nested
        public struct EntityContext
        {
            public CaseCircularImmediate0 Main1 { get; private set; }
            public CaseCircularImmediate1 Main2 { get; private set; }
            public CallsRegister Calls { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main1 = entity.FetchFirst<CaseCircularImmediate0>(),
                    Main2 = entity.FetchFirst<CaseCircularImmediate1>(),
                    Calls = entity.FetchFirst<CallsRegister>(),
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;
        [Test]
        public void InjectsCircularDependenciesInAnyOrder()
        {
            var entity = _fixtures.InstantiateEntityPrefab("CircularImmediate");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main1, Is.Not.Null);
            Assert.That(ctx.Main2, Is.Not.Null);

            Assert.That(ctx.Main1.Circular, Is.EqualTo(ctx.Main2));
            Assert.That(ctx.Main2.Circular, Is.EqualTo(ctx.Main1));
        }

        [Test]
        public void OnlyCallsAutofetchOnce()
        {
            var entity = _fixtures.InstantiateEntityPrefab("CircularImmediate");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main1, Is.Not.Null);
            Assert.That(ctx.Main2, Is.Not.Null);

            Assert.That(ctx.Calls.Count(typeof(CaseCircularImmediate0)), Is.EqualTo(1));
            Assert.That(ctx.Calls.Count(typeof(CaseCircularImmediate1)), Is.EqualTo(1));
        }
    }
}
