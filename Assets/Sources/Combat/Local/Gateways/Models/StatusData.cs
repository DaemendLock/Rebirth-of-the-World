using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;

namespace Combat.Local.Data.Models
{
    public readonly struct ActionData
    {
        public ActionData(AnimationClip animation, IFrameData frameData)
        {
            Animation = animation;
            FrameData = frameData;
        }

        public readonly AnimationClip Animation { get; }
        public readonly IFrameData FrameData { get; }
    }

    public readonly struct ActorData
    {
        public ActorData(ActorState state, IAction action)
        {
            State = state;
            Action = action;
        }

        public ActorState State { get; }

        public IAction Action { get; }
    }

    public readonly struct StatusData
    {
        public EntityId Parent { get; }
        public StatusName Name { get; }
        public EntityId? Caster { get; }
        public SkillId? Source { get; }

        public int StackCount { get; }
        public Duration Duration { get; }
    }
}
