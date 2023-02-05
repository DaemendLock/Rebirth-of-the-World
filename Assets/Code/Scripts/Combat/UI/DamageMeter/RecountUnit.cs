using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecountUnit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _ammountText;

    private Unit _associatedUnit;

    private float _damageAmmount;
    private float _healintAmmount;

    public float DamageAmmount => _damageAmmount;
    public float HealingAmmount => _healintAmmount;

    public void UpdateHeal(AttackEventInstance e) {
        _damageAmmount += e.damage;
    }

    public void UpdateDamage(AttackEventInstance e) {
        _healintAmmount -= e.damage;
    }



}
