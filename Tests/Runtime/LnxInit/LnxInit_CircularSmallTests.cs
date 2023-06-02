using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;

namespace LnxArch.Tests
{
    public class LnxInit_CircularSmallTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseCircularSmall0 Main1 { get; private set; }
            public CaseCircularSmall1 Main2 { get; private set; }
            public CaseCircularSmall2 Main3 { get; private set; }
            public CallsRegister Calls { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main1 = entity.FetchFirst<CaseCircularSmall0>(),
                    Main2 = entity.FetchFirst<CaseCircularSmall1>(),
                    Main3 = entity.FetchFirst<CaseCircularSmall2>(),
                    Calls = entity.FetchFirst<CallsRegister>(),
                };
            }
        }
        #endregion
        private static readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        private const string PrefabName = "CircularSmall";
        [Test]
        public void InjectsCircularDependenciesInAnyOrder()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main1, Is.Not.Null);
            Assert.That(ctx.Main2, Is.Not.Null);
            Assert.That(ctx.Main3, Is.Not.Null);

            Assert.That(ctx.Main1.Circular, Is.EqualTo(ctx.Main2));
            Assert.That(ctx.Main2.Circular, Is.EqualTo(ctx.Main3));
            Assert.That(ctx.Main3.Circular, Is.EqualTo(ctx.Main1));
        }

        [Test]
        public void OnlyInitializesOnce()
        {
            var entity = _fixtures.InstantiateEntityPrefab(PrefabName);
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main1, Is.Not.Null);
            Assert.That(ctx.Main2, Is.Not.Null);
            Assert.That(ctx.Main3, Is.Not.Null);

            Assert.That(ctx.Calls.Count(typeof(CaseCircularSmall0)), Is.EqualTo(1));
            Assert.That(ctx.Calls.Count(typeof(CaseCircularSmall1)), Is.EqualTo(1));
            Assert.That(ctx.Calls.Count(typeof(CaseCircularSmall2)), Is.EqualTo(1));
        }
    }
}
