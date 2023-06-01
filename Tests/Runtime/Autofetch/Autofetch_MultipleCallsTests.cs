using NUnit.Framework;
using LnxArch.TestTools;
using LnxArch.TestFixtures;
using UnityEngine;

namespace LnxArch.Tests
{
    public class Autofetch_MultipleCallsTests
    {
        #region Nested
        public struct EntityContext
        {
            public CaseMultipleMethods1 Main1 { get; private set; }
            public CaseMultipleMethods2 Main2 { get; private set; }
            public CaseMultipleMethods3 Main3 { get; private set; }
            public BoxCollider BoxCollider { get; private set; }
            public SphereCollider SphereCollider { get; private set; }
            public CapsuleCollider CapsuleCollider { get; private set; }
            public CallsRegister CallsRegister { get; private set; }
            public int Main1Prepare1OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods1), nameof(CaseMultipleMethods1.Prepare1));
            public int Main1Prepare2OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods1), nameof(CaseMultipleMethods1.Prepare2));
            public int Main2Prepare1OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods2), nameof(CaseMultipleMethods2.Prepare1));
            public int Main2Prepare2OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods2), nameof(CaseMultipleMethods2.Prepare2));
            public int Main3Prepare1OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods3), nameof(CaseMultipleMethods3.Prepare1));
            public int Main3Prepare2OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods3), nameof(CaseMultipleMethods3.Prepare2));
            public int Main3Prepare3OrderInx => CallsRegister.OrderOf(typeof(CaseMultipleMethods3), nameof(CaseMultipleMethods3.Prepare3));

            public static EntityContext Create(LnxEntity entity)
            {
                return new EntityContext {
                    Main1 = entity.FetchFirst<CaseMultipleMethods1>(),
                    Main2 = entity.FetchFirst<CaseMultipleMethods2>(),
                    Main3 = entity.FetchFirst<CaseMultipleMethods3>(),
                    BoxCollider = entity.FetchFirst<BoxCollider>(),
                    SphereCollider = entity.FetchFirst<SphereCollider>(),
                    CapsuleCollider = entity.FetchFirst<CapsuleCollider>(),
                    CallsRegister = entity.FetchFirst<CallsRegister>(),
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeAutofetch;
        [Test]
        public void CallAllInitializationMethods()
        {
            var entity = _fixtures.InstantiateEntityPrefab("MultipleMethods");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main1, Is.Not.Null);
            Assert.That(ctx.Main2, Is.Not.Null);
            Assert.That(ctx.Main3, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.Main1.BoxCollider, Is.EqualTo(ctx.BoxCollider));
            Assert.That(ctx.Main1.SphereCollider, Is.EqualTo(ctx.SphereCollider));
            Assert.That(ctx.Main2.BoxCollider, Is.EqualTo(ctx.BoxCollider));
            Assert.That(ctx.Main2.SphereCollider, Is.EqualTo(ctx.SphereCollider));
            Assert.That(ctx.Main3.BoxCollider, Is.EqualTo(ctx.BoxCollider));
            Assert.That(ctx.Main3.SphereCollider, Is.EqualTo(ctx.SphereCollider));
            Assert.That(ctx.Main3.CapsuleCollider, Is.EqualTo(ctx.CapsuleCollider));
        }
        [Test]
        public void CallDependenciesFirst()
        {
            var entity = _fixtures.InstantiateEntityPrefab("MultipleMethods");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Main1, Is.Not.Null);
            Assert.That(ctx.Main2, Is.Not.Null);
            Assert.That(ctx.Main3, Is.Not.Null);
            Assert.That(ctx.BoxCollider, Is.Not.Null);
            Assert.That(ctx.SphereCollider, Is.Not.Null);
            Assert.That(ctx.CapsuleCollider, Is.Not.Null);

            Assert.That(ctx.Main1Prepare1OrderInx, Is.GreaterThan(ctx.Main2Prepare1OrderInx));
            Assert.That(ctx.Main1Prepare1OrderInx, Is.GreaterThan(ctx.Main2Prepare2OrderInx));
            Assert.That(ctx.Main1Prepare2OrderInx, Is.GreaterThan(ctx.Main3Prepare1OrderInx));
            Assert.That(ctx.Main1Prepare2OrderInx, Is.GreaterThan(ctx.Main3Prepare2OrderInx));
            Assert.That(ctx.Main1Prepare2OrderInx, Is.GreaterThan(ctx.Main3Prepare3OrderInx));
        }
    }
}
