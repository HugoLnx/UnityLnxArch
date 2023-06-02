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
    public class LnxInit_ComponentInjectedInManyTests
    {
        #region Nested
        public struct EntityContext
        {
            public CallsRegister Calls { get; private set; }
            public CaseComponentInjectedInMany_Receiver Receiver1 { get; private set; }
            public CaseComponentInjectedInMany_Receiver Receiver2 { get; private set; }
            public CaseComponentInjectedInMany_Target Target { get; private set; }
            public CaseComponentInjectedInMany_ReceiverNested ReceiverNested { get; private set; }
            public int Receiver1CallInx => Calls.OrderOf(typeof(CaseComponentInjectedInMany_Receiver), nameof(Receiver1.Prepare));
            public int Receiver2CallInx => Calls.OrderOf(typeof(CaseComponentInjectedInMany_Receiver), nameof(Receiver2.Prepare));
            public int TargetCallInx => Calls.OrderOf(typeof(CaseComponentInjectedInMany_Target), nameof(Target.Prepare));
            public int ReceiverNestedCallInx => Calls.OrderOf(typeof(CaseComponentInjectedInMany_ReceiverNested), nameof(ReceiverNested.Prepare));
            public Collider Collider { get; private set; }

            public static EntityContext Create(LnxEntity entity)
            {
                var receivers = entity.FetchAll<CaseComponentInjectedInMany_Receiver>();
                return new EntityContext {
                    Calls = entity.FetchFirst<CallsRegister>(),
                    Receiver1 = receivers[0],
                    Receiver2 = receivers.Length > 1 ? receivers[1] : null,
                    Target = entity.FetchFirst<CaseComponentInjectedInMany_Target>(),
                    ReceiverNested = entity.FetchFirst<CaseComponentInjectedInMany_ReceiverNested>(),
                    Collider = entity.FetchFirst<Collider>()
                };
            }
        }
        #endregion
        private readonly FixturesLoader _fixtures = FixturesLoader.RuntimeLnxInit;
        [Test]
        public void Simple_TargetInitializationMethodIsCalledOnlyOnce()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ComponentInjectedInMany_Simple");
            EntityContext ctx = EntityContext.Create(entity);

            int targetPrepareCallsCount = ctx.Calls.Calls.Count(callName => callName.Contains("Target"));

            Assert.That(targetPrepareCallsCount, Is.EqualTo(1));
        }

        [Test]
        public void Simple_TargetIsInjectedInAllReceivers()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ComponentInjectedInMany_Simple");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Receiver1, Is.Not.Null);
            Assert.That(ctx.Target, Is.Not.Null);
            Assert.That(ctx.Collider, Is.Not.Null);

            Assert.That(ctx.Receiver1.Target, Is.EqualTo(ctx.Target));
            Assert.That(ctx.Receiver2.Target, Is.EqualTo(ctx.Target));
            Assert.That(ctx.Target.Collider, Is.EqualTo(ctx.Collider));
        }
        [Test]
        public void Nested_TargetInitializationMethodIsCalledOnlyOnce()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ComponentInjectedInMany_Nested");
            EntityContext ctx = EntityContext.Create(entity);

            int targetPrepareCallsCount = ctx.Calls.Calls.Count(callName => callName.Contains("Target"));

            Assert.That(targetPrepareCallsCount, Is.EqualTo(1));
        }

        [Test]
        public void Nested_TargetIsInjectedInAllReceivers()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ComponentInjectedInMany_Nested");
            EntityContext ctx = EntityContext.Create(entity);
            Assert.That(ctx.Receiver1, Is.Not.Null);
            Assert.That(ctx.ReceiverNested, Is.Not.Null);
            Assert.That(ctx.Target, Is.Not.Null);
            Assert.That(ctx.Collider, Is.Not.Null);

            Assert.That(ctx.Receiver1.Target, Is.EqualTo(ctx.Target));
            Assert.That(ctx.ReceiverNested.Target, Is.EqualTo(ctx.Target));
            Assert.That(ctx.ReceiverNested.Receiver, Is.EqualTo(ctx.Receiver1));
            Assert.That(ctx.Target.Collider, Is.EqualTo(ctx.Collider));
        }

        [Test]
        public void Nested_DependenciesAreCalledFirst()
        {
            var entity = _fixtures.InstantiateEntityPrefab("ComponentInjectedInMany_Nested");
            EntityContext ctx = EntityContext.Create(entity);

            // Nested receiver dependencies
            Assert.That(ctx.ReceiverNestedCallInx, Is.GreaterThan(ctx.TargetCallInx));
            Assert.That(ctx.ReceiverNestedCallInx, Is.GreaterThan(ctx.Receiver1CallInx));

            // Receiver dependencies
            Assert.That(ctx.Receiver1CallInx, Is.GreaterThan(ctx.TargetCallInx));
        }
    }
}
