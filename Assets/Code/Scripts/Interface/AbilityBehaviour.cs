
namespace Abilities {
    public interface ICastable {

        void OnCastStart();

        void OnCastThink(float casttime);

        void OnCastFinished(bool succes);
    }

    public interface IChannelable {

        void OnChannelStart();

        void OnChannelThink(float casttime);

        void OnChannelFinished(bool succes);

    }

    public interface ITargetable {
        float CastRadius { get; }
        
    }

    public interface IUnitTarget : ITargetable {
        Unit CursorTarget { get; }
        UNIT_TARGET_FLAGS TargetFlag { get; }
        UNIT_TARGET_TEAM TargetTeam { get; }
        UnitFilterResult CastFilterResultTarget(Unit target);

        bool GetCursorTargetingNothing();
    }

    

}
