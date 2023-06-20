namespace Combat.SpellOld
{
    public abstract class UnitTargetAbility : TargetableAbility, IUnitTarget
    {

        public abstract UnitTargetFlag TargetFlag { get; }

        public abstract UnitTargetTeam TargetTeam { get; }

        private Unit _target;

        public UnitTargetAbility(Unit owner) : base(owner)
        {

        }

        public Unit CursorTarget => _target;

        public bool GetCursorTargetingNothing() => CursorTarget == null;

        public virtual UnitFilterResult CastFilterResultTarget(Unit target)
        {
            if (target == null)
                return UnitFilterResult.UF_FAIL_INVALID_LOCATION;

            if (RotW.CheckDistance(target, Owner, CastRadius) == false)
                return UnitFilterResult.UF_FAIL_INVALID_LOCATION;

            if (target.Dead && ((int) TargetFlag & (int) UnitTargetFlag.DEAD) == 0)
                return UnitFilterResult.UF_FAIL_DEAD;

            if (TargetTeam == UnitTargetTeam.BOTH)
                return UnitFilterResult.UF_SUCCESS;

            if (target.Team == Owner.Team)
            {
                if (TargetTeam == UnitTargetTeam.FRIENDLY)
                    return UnitFilterResult.UF_SUCCESS;
                return UnitFilterResult.UF_FAIL_FRIENDLY;
            }

            if (TargetTeam != UnitTargetTeam.ENEMY)
                return UnitFilterResult.UF_FAIL_ENEMY;

            return UnitFilterResult.UF_SUCCESS;
        }

        public sealed override UnitFilterResult StartAbility()
        {
            UnitFilterResult result;
            _target = Controller.GetCursorTarget();

            result = CastFilterResultTarget(_target);
            if (result != UnitFilterResult.UF_SUCCESS)
            {
                _target = null;
                return result;
            }
            OnSpellStart();
            if (!Castable)
            {
                PayManaCost();
                StartCooldown();
            }

            return UnitFilterResult.UF_SUCCESS;
        }
    }
}