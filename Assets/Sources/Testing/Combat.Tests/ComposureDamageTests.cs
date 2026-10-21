using Combat.API.Utils;
using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Facades;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Gateways.Repositories;
using Combat.Local.Gateways.Repositories.Characters;

using NUnit.Framework;

using System.Collections.Generic;

namespace Combat.Tests
{
    public sealed class ComposureDamageTests
    {
        [Test]
        public void DamageReducesBothResourcesAndReportsBothValues()
        {
            Scenario scenario = new();
            scenario.Apply(30f, 20f);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(70f));
            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(80f));
            Assert.That(scenario.Output.Damage.OriginalHealthDamage, Is.EqualTo(30f));
            Assert.That(scenario.Output.Damage.OriginalComposureDamage, Is.EqualTo(20f));
            Assert.That(scenario.Output.CurrentHealth, Is.EqualTo(70f));
            Assert.That(scenario.Output.CurrentComposure, Is.EqualTo(80f));
            Assert.That(scenario.Results.HealthDamage, Is.EqualTo(30f));
            Assert.That(scenario.Results.ComposureDamage, Is.EqualTo(20f));
            Assert.That(scenario.Results.IncomingCalls, Is.EqualTo(1));
            Assert.That(scenario.Results.OutgoingCalls, Is.EqualTo(1));
        }

        [Test]
        public void CombinedReductionIsCalculatedOnceAndAppliedToBothDamageValues()
        {
            Scenario scenario = new();
            scenario.Modifiers.IncomingPercent = 60f;
            scenario.Modifiers.OutgoingPercent = 40f;
            scenario.Apply(80f, 40f);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(60f));
            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(80f));
            Assert.That(scenario.Modifiers.IncomingCalls, Is.EqualTo(1));
            Assert.That(scenario.Modifiers.OutgoingCalls, Is.EqualTo(1));
            Assert.That(scenario.Modifiers.OriginalHealthDamage, Is.EqualTo(80f));
            Assert.That(scenario.Modifiers.OriginalComposureDamage, Is.EqualTo(40f));
        }

        [Test]
        public void FlatReductionAppliesToBothValuesAndCannotHeal()
        {
            Scenario scenario = new();
            scenario.Modifiers.BaseValue = -10f;
            scenario.Modifiers.BonusValue = -5f;
            scenario.Apply(40f, 10f);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(75f));
            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(100f));
            Assert.That(scenario.Results.ComposureDamage, Is.Zero);
        }

        [Test]
        public void ComposureOnlyDamageCanDepleteComposureWithoutKilling()
        {
            Scenario scenario = new();
            scenario.Apply(0f, 150f);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(100f));
            Assert.That(scenario.Health.CurrentComposure, Is.Zero);
            Assert.That(scenario.Results.ComposureDamage, Is.EqualTo(100f));
            Assert.That(scenario.Actors.State, Is.EqualTo(ConsciousState.Alive));
            Assert.That(scenario.Output.Deaths, Is.Zero);

            scenario.Apply(0f, 10f);
            Assert.That(scenario.Results.ComposureDamage, Is.Zero);
        }

        [Test]
        public void HealthOnlyDamageLeavesComposureUnchanged()
        {
            Scenario scenario = new();
            scenario.Apply(25f, 0f);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(75f));
            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(100f));
        }

        [Test]
        public void NonLethalDamageProtectsHealthAndStillDepletesComposure()
        {
            Scenario scenario = new(15f, 50f);
            scenario.Apply(100f, 100f, DamageFlags.NonLethal);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(1f));
            Assert.That(scenario.Health.CurrentComposure, Is.Zero);
            Assert.That(scenario.Output.Damage.HealthDamage, Is.EqualTo(14f));
            Assert.That(scenario.Results.HealthDamage, Is.EqualTo(14f));
            Assert.That(scenario.Actors.State, Is.EqualTo(ConsciousState.Alive));
        }

        [Test]
        public void HealthDepletionStillKillsWhenComposureRemains()
        {
            Scenario scenario = new(10f, 100f);
            scenario.Apply(20f, 5f);

            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(95f));
            Assert.That(scenario.Actors.State, Is.EqualTo(ConsciousState.Dead));
            Assert.That(scenario.Output.Deaths, Is.EqualTo(1));
        }

        [Test]
        public void NegativeDamageCannotRestoreEitherResource()
        {
            Scenario scenario = new(50f, 50f);
            scenario.Apply(-20f, -30f);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(50f));
            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(50f));
            Assert.That(scenario.Results.HealthDamage, Is.Zero);
            Assert.That(scenario.Results.ComposureDamage, Is.Zero);
        }

        [Test]
        public void NonReactableDamageStillAppliesReductionToBothResources()
        {
            Scenario scenario = new();
            scenario.Modifiers.IncomingPercent = 100f;
            scenario.Apply(20f, 40f, DamageFlags.NonReactable);

            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(90f));
            Assert.That(scenario.Health.CurrentComposure, Is.EqualTo(80f));
            Assert.That(scenario.Results.IncomingCalls, Is.Zero);
            Assert.That(scenario.Results.OutgoingCalls, Is.Zero);
        }

        [Test]
        public void ComposureCanBeSetAndRestoredThroughFacadeWithinItsBounds()
        {
            Scenario scenario = new();
            HealthOwnerFacade facade = new(new(scenario.Repository, scenario.Output), scenario.UseCase, null, scenario.Repository);

            facade.SetComposure(Scenario.Target, -20f);
            Assert.That(facade.GetHealth(Scenario.Target).CurrentComposure, Is.Zero);
            facade.SetComposure(Scenario.Target, 150f);
            Assert.That(facade.GetHealth(Scenario.Target).CurrentComposure, Is.EqualTo(100f));
            Assert.That(facade.GetHealth(Scenario.Target).MaxComposure, Is.EqualTo(100f));
            Assert.That(scenario.Output.CurrentComposure, Is.EqualTo(100f));
            Assert.That(scenario.Health.CurrentHealth, Is.EqualTo(100f));
        }

        [Test]
        public void ExistingDamageCallsRemainHealthOnlyAndOptionsCarryBothValues()
        {
            Combat.API.DTO.ApplyDamageInfo legacy = new(null, null, 25f, DamageFlags.None);
            Assert.That(legacy.HealthDamage, Is.EqualTo(25f));
            Assert.That(legacy.ComposureDamage, Is.Zero);

            ApplyDamageOptions options = new(null, null, null, 40f, 30f, DamageFlags.NonLethal);
            Assert.That(options.AsDamageData.HealthDamage, Is.EqualTo(40f));
            Assert.That(options.AsDamageData.ComposureDamage, Is.EqualTo(30f));
            Assert.That(options.AsDamageData.Flags, Is.EqualTo(DamageFlags.NonLethal));
        }

        private sealed class Scenario
        {
            public static readonly UnitId Target = new(1);
            private static readonly UnitId Attacker = new(2);
            public readonly DictionaryHealthRepository Repository = new();
            public readonly Output Output = new();
            public readonly Modifiers Modifiers = new();
            public readonly Results Results = new();
            public readonly Actors Actors = new();
            public readonly HealthApplyDamageUseCase UseCase;

            public Scenario(float currentHealth = 100f, float currentComposure = 100f)
            {
                Repository.Create(new(Target, currentHealth, 100f, currentComposure, 100f));
                StatusOwnerRepository statuses = new();
                statuses.Create(new(Target));
                statuses.Create(new(Attacker));
                UseCase = new(Repository, Output, Actors, statuses, Output, Modifiers, Results, Output);
            }

            public HealthOwner Health => Repository.Get(Target);

            public void Apply(float healthDamage, float composureDamage, DamageFlags flags = DamageFlags.None) =>
                UseCase.Execute(new(Target, healthDamage, composureDamage, flags, Attacker, null));
        }

        private sealed class Output : IHealthOutput, ICharacterConsciousStateOutput, ICharacterDeathHandler
        {
            public DamageInstance Damage;
            public float CurrentHealth;
            public float CurrentComposure;
            public int Deaths;

            public void Present(DamageInstance instance) => Damage = instance;
            public void Present(HealthChangeRecord record) => CurrentHealth = record.CurrentHealth;
            public void Present(ComposureChangeRecord record) => CurrentComposure = record.CurrentComposure;
            public void Present(UnitId id, ConsciousState state) { }
            public void Handle(KillRecord record) => Deaths++;
        }

        private sealed class Modifiers : IHealingDamageModificationCalculator
        {
            public float IncomingPercent;
            public float OutgoingPercent;
            public float BaseValue;
            public float BonusValue;
            public int IncomingCalls;
            public int OutgoingCalls;
            public float OriginalHealthDamage;
            public float OriginalComposureDamage;

            public DamageModification GetIncomingDamageModification(IEnumerable<StatusId> statuses, in DamageInstance instance)
            {
                IncomingCalls++;
                OriginalHealthDamage = instance.OriginalHealthDamage;
                OriginalComposureDamage = instance.OriginalComposureDamage;
                return new(BaseValue, IncomingPercent, BonusValue, DamageFlags.None);
            }

            public DamageModification GetOutgoingDamageModification(IEnumerable<StatusId> statuses, in DamageInstance instance)
            {
                OutgoingCalls++;
                return new(0f, OutgoingPercent, 0f, DamageFlags.None);
            }

            public HealingModification GetOutgoingHealingModification(IEnumerable<StatusId> statuses, in HealingInstance instance) => default;
        }

        private sealed class Results : IDamageResultHandler
        {
            public float HealthDamage;
            public float ComposureDamage;
            public int IncomingCalls;
            public int OutgoingCalls;

            public void NotifyDamageRecieved(DamageRecord record, IEnumerable<StatusId> handlers)
            {
                IncomingCalls++;
                HealthDamage = record.FinalHealthDamage;
                ComposureDamage = record.FinalComposureDamage;
            }

            public void NotifyDamageDealth(DamageRecord record, IEnumerable<StatusId> handlers) => OutgoingCalls++;
        }

        private sealed class Actors : IActorRepository
        {
            public ConsciousState State = ConsciousState.Alive;
            public void Create(Actor actor) => State = actor.ConsciousState;
            public void Update(Actor actor) => State = actor.ConsciousState;
            public void Delete(UnitId id) { }

            public bool TryGet(UnitId id, out Actor actor)
            {
                actor = new(id, default, null, State, default);
                return true;
            }
        }
    }
}
