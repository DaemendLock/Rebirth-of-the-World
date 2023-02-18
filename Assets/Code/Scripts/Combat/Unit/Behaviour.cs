
/*

public enum UnitState : byte {
    IDLE,
    MOVING,
    CASTING,
    ATTACKING,
    DEAD
}

public interface IBehaviour {
    UnitState State { get; }

    void Process(float time);
}

public class IdleBehavoiur : IBehaviour {
    public UnitState State => UnitState.IDLE;

    public void Process(float time) {

    }
}

public class CastingBehavoiur : IBehaviour {
    private Unit _caster;
    private Ability _cast;
    private float _castTime = 0;
    private float _channelTime = 0;
    private bool _isCasting = false;
    private bool _isChannel = false;

    public CastingBehavoiur(Unit caster) {
        _caster = caster;
    }

    public UnitState State => UnitState.CASTING;

    public float CastTimeRemain => _castTime;

    public float ChannelTime => _channelTime;

    public bool Casting => _isCasting;

    public bool Channeling => _isChannel;

    public Ability CurrentCastAbility => _cast;

    public void Process(float time) {
        if (_isCasting)
            ProcessCast(time);
        else if (_isChannel)
            ProcessChannel(time);
    }

    public void StartCasting(Ability ability) {
        if (!ability.Castable)
            return;
        _isCasting = true;
        _cast = ability;
        _castTime = ability.CastTime * _caster.HasteCasttimeModification;
        ability.OnCastStart();
        _caster.CastStarted?.Invoke(ability);
    }

    public void StopCasting(bool succes) {
        _isCasting = false;
        _caster._target = null;
        _cast?.OnCastFinished(succes);
        _caster.CastEnded?.Invoke(_cast, succes);
        if (succes && _cast.Channelable) {
            StartChannel(_cast);
            return;
        }
        _cast = null;
        if (succes && _caster.QueuedAbility != null) {
            _caster.QueuedAbility.CastAbility();
            _caster._queuedAbility = null;
        }
    }

    public void StartChannel(Ability ability) {
        _isChannel = true;
        _channelTime = 0;
        _cast = ability;
        _caster.ChannelStarted?.Invoke(ability);
        ability.OnChannelStart();
    }

    public void StopChannel(bool succes) {
        _isChannel = false;
        _channelTime = 0;
        _caster.ChannelEnded?.Invoke(_cast, succes);
        _cast = null;

        if (succes && _caster.QueuedAbility != null) {
            _caster.QueuedAbility.CastAbility();
            _caster._queuedAbility = null;
        }
    }

    private void ProcessCast(float time) {
        _castTime -= time;
        _cast?.OnCastThink(_castTime);
        _caster.CastProcessed?.Invoke(time);
        if (_cast != null && _castTime < 0) {
            StopCasting(true);
        }
    }

    private void ProcessChannel(float deltaTime) {
        _channelTime += deltaTime;
        _cast?.OnChannelThink(ChannelTime);
        _caster.ChannelProcessed?.Invoke(deltaTime);

        if (_cast != null && _channelTime >= _cast.ChannelTime) {
            StopChannel(true);
        }
    }
}*/


