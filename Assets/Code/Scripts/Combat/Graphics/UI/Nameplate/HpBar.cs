using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Nameplate))]
public class HpBar : RefreshableUI, IUnitBindable {
    [SerializeField] private Slider _bar;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillingImage;
    [Space(10)]
    [Header("Color settings")]
    private Color _allyColor;
    [SerializeField] private Color _allyBackgroundColor;
    private Color _enemyColor;
    [SerializeField] private Color _enemyBackgroundColor;

    private Unit _owner;

    public void BindUnit(Unit unit) => _owner = unit;

    private void Awake() {
        GetComponent<Nameplate>().SubscribeForOwnerBinding(this);
    }

    private void Start() {
        _owner.HealthChanged += UpdateHealthbar;
        _owner.TeamChanged += ChangeTeam;
        UpdateUIData();
    }

    private void OnDestroy() {
        _owner.HealthChanged -= UpdateHealthbar;
        _owner.TeamChanged -= ChangeTeam;
    }

    private void UpdateHealthbar(HealthChangeEventInstance health) {
        _bar.value = _owner.HealthPercent;
    }

    private void ChangeTeam(Unit unit, Team prevTeam) {
        _fillingImage.color = _owner.Team == Team.ALLY ? _allyColor : _enemyColor;
        _backgroundImage.color = _owner.Team == Team.ALLY ? _allyBackgroundColor : _enemyBackgroundColor;
    }

    public override void UpdateUIData() {
        _allyColor = Lobby.ActiveAccount.Data.Settings.Nameplates.AllyHealthbarColor;
        _enemyColor = Lobby.ActiveAccount.Data.Settings.Nameplates.EnemyHealthbarColor;
        _fillingImage.color = _owner.Team == Team.ALLY ? _allyColor : _enemyColor;
        _backgroundImage.color = _owner.Team == Team.ALLY ? _allyBackgroundColor : _enemyBackgroundColor;
    }

    public override void Hide() {
        
    }

    public override void Show() {
        
    }
}
