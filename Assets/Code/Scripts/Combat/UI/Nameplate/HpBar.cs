using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider _bar;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillingImage;
    [Space(10)]
    [Header("Color settings")]
    [SerializeField] private Color allyColor;
    [SerializeField] private Color allyBackgroundColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color enemyBackgroundColor;

    private Unit _owner;

    private void Awake() {
        _owner = GetComponent<Nameplate>().Owner;
    }

    private void Start() {
        _owner.HealthChanged += UpdateHealthbar;
        _owner.RecivedDamage += UpdateHealthbar;
        _owner.Healed += UpdateHealthbar;
        _owner.TeamChange += ChangeTeam;
    }

    private void OnDestroy() {
        _owner.HealthChanged -= UpdateHealthbar;
        _owner.RecivedDamage -= UpdateHealthbar;
        _owner.Healed -= UpdateHealthbar;
        _owner.TeamChange -= ChangeTeam;
    }

    private void UpdateHealthbar(float health) {
        _bar.value = _owner.HealthPercent;
    }

    private void UpdateHealthbar(AttackEventInstance e) {
        UpdateHealthbar(0);
    }
    private void ChangeTeam(Unit unit, Team prevTeam) {
        _fillingImage.color = _owner.Team == Team.TEAM_ALLY ? allyColor : enemyColor;
        _backgroundImage.color = _owner.Team == Team.TEAM_ALLY ? allyBackgroundColor : enemyBackgroundColor;
    }

}
