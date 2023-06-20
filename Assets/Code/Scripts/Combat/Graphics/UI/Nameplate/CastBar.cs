using Combat.SpellOld;
using UnityEngine;
using UnityEngine.UI;

public class CastBar : RefreshableUI, IUnitBindable {
    [SerializeField] private Slider _bar;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _progressBar;
    [SerializeField] private Image _castIcon;
    [SerializeField] private GameObject _contentBox;
    [Space(10)]
    [Header("Cast colors")]
    private Color _castColor;
    private Color _channelColor;

    private Unit _owner;

    private OldAbility _castAbility;

    private void Awake() {
        GetComponent<Nameplate>().SubscribeForOwnerBinding(this);
        UpdateUIData();
    }


    public void BindUnit(Unit owner) => _owner = owner;

    private void Start() {
        _owner.CastStarted += EnableCastbar;
        _owner.CastProcessed += UpdateCast;
        _owner.CastEnded += DisableCastbar;
        _owner.ChannelStarted += EnableChnnel;
        _owner.ChannelProcessed += UpdateChannel;
        _owner.ChannelEnded += DisableCastbar;
    }

    private void OnDestroy() {
        _owner.CastStarted -= EnableCastbar;
        _owner.CastProcessed -= UpdateCast;
        _owner.CastEnded -= DisableCastbar;
        _owner.ChannelStarted -= EnableChnnel;
        _owner.ChannelProcessed -= UpdateChannel;
        _owner.ChannelEnded -= DisableCastbar;
    }

    private void UpdateCast(float time) {
        _bar.value = _owner.CastTimeRemain / (_castAbility.CastTime / _owner.HasteCasttimeDivider);
    }
    private void UpdateChannel(float time) {
        if (_castAbility == null)
            return;
        _bar.value = _owner.ChannelTime / _castAbility.ChannelTime;
    }


    private void EnableCastbar(OldAbility ability) {
        _progressBar.color = _castColor;
        _progressBar.fillAmount = 1;
        Enable(ability);
    }

    private void EnableChnnel(OldAbility ability) {
        _progressBar.color = _channelColor;
        _progressBar.fillAmount = 0;
        Enable(ability);
    }

    private void DisableCastbar(OldAbility ability, bool succes) {
        Disable();
    }

    private void Enable(OldAbility ability) {
        _castAbility = ability;
        _castIcon.sprite = ability.AbilityIcon;
        _contentBox.SetActive(true);
    }

    private void Disable() {
        _contentBox.SetActive(false);
        _castAbility = null;
    }

    public override void UpdateUIData() {
        _castColor = Lobby.ActiveAccount.Data.Settings.Nameplates.InterruptibleCastColor;
        _channelColor = Lobby.ActiveAccount.Data.Settings.Nameplates.ChannelColor;
    }

    public override void Hide() {

    }

    public override void Show() {

    }
}
