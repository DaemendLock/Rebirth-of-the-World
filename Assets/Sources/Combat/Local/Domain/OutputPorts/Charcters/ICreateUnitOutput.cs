using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System;

using UnityEngine;

namespace Combat.Local.Domain.OutputPorts
{
    public interface ICreateUnitOutput
    {
        void Present(Positionable positionable, Transform parent = null);
    }

    public interface ICreateUnitEventHandler
    {
        void HandleEvent(EntityId id, ReadOnlySpan<SkillId> initalSkills);
    }

    public interface IApplyStatusOutput
    {
        void Present(Status status);
    }

    public interface IApplyStatusEventHandler
    {
        void HandleEvent(Status status);
    }

    public interface IGiveSkillEventHandler
    {
        void HandleEvent(EntityId target, SkillId skill);
    }

    public interface ISpendResourceOutput
    {
        void Present(Resource resource);
    }

    public interface IActionStateChangeEventHandler
    {
        void HandleEvent(EntityId actorId, ActionState newState);
    }
}
