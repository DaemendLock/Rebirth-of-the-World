using UnityEngine;

public class FlorenceTrial : Scenario
{

    //public Sprite scenarioImage = RotW.sprites["FlorenceScenarioImage"];

    private Unit Florence;

    public override UnitPreview[] GetUnitsPreset() => new UnitPreview[] { new UnitPreview(RotW.GetUnitDataById(0), null, null, 0, 0) };

    public override SpawnLocation[] GetSpawnLocations() => new SpawnLocation[] { new SpawnLocation(Vector3.zero, Quaternion.Euler(0, 90, 0)) };

    public override GameObject Terrain => Resources.Load("Terrain/Cube") as GameObject;


    public override IQuest[] Goals => new IQuest[] {
                            new ReadTipQuest("Welcome to Florence Trial. Here you can learn how to unleash Florence potential."),
                            new ReadTipQuest("Florence is Caster-type Support, which can povide various buffs and heal allies."),
                            new ReadTipQuest("First of all, let's get acquainted with Florence's abilities. \"Relife pleasure\" can be casted on an ally. When cast ends, target get effect of one of Florence's special medicine: Healing overtime, Instant damage and HoT for twice damage ammount, DoT and haste buff. Give it a try!"),
                            new CastQuest(2),
                            new ReadTipQuest("Florence is master of nursing, she can take care of multiple allies in the same time! While castong \"Nurse care\" Florence heals ally with lowest percentage of health and apply 1 stack of \"Nursing\""),
                            new CastQuest(1),
                            new ReadTipQuest("But sometimes patient needs \"Special attention\". Casting it on an ally heals depends on missing health percentage and also applies 5 stacks of Nursing. When unit effected by Nursing and takes damage, damage is decresed by 18% per stack and unit loses 1 stack of the effect."),
                            new CastQuest(3),
                            new ReadTipQuest("Well, taking care of all those folks are pretty boring, so Florence passivly gain her seconds resource \"Excitement\". If Excitement is full, Florence is overwhelmed with emotions, so loses some of combat potential as mana"),
                            new ReadTipQuest("\"White imp\" allows Florence to convert some of Excitement to mana, while taking some of \"Sweet pills\" instantly resores Excitement to maximum."),
                            new ReadTipQuest("Now let's talk about \"Refreshing concoction\". Florence injects ally or self with special medicine, which instanly takes 60% of target current health, while increasing attack and spellpower by 60% for 7 sec. It also brings Excitement to zero"),
                            new CastQuest(0),
                            new ReadTipQuest("To finish this scenario fully heal your ally."),
                            null
    };

    private Unit _tempUnit;

    public override IQuest NextQuest(int i)
    {

        switch (i)
        {

            case 0:
                return Goals[0];

            case 1:
                return Goals[1];

            case 2:
                Controller.Instance.ChooseNextAlly();
                Florence = Controller.Instance.SelectedUnit;
                return Goals[2];

            case 3:
                //_tempUnit = RotW.CreateUnitByType(RotW.UnitFactory.UnitType.FLORENCE, new Vector3(-15, 0, 5), Team.TEAM_ALLY);
                _tempUnit.SetHealth(_tempUnit.CurrentHealth * 0.5f);
                Goals[13] = new HealQuest(_tempUnit);
                UI.Combat.UI.Instance.paused = false;
                return Goals[3];

            case 4:
                UI.Combat.UI.Instance.paused = true;
                return Goals[4];

            case 5:
                UI.Combat.UI.Instance.paused = false;
                return Goals[5];

            case 6:
                _tempUnit.SetHealth(_tempUnit.CurrentHealth * 0.5f);
                UI.Combat.UI.Instance.paused = false;
                return Goals[6];

            case 7:
                UI.Combat.UI.Instance.paused = false;
                return Goals[7];

            case 8:
                _tempUnit.SetHealth(_tempUnit.CurrentHealth * 0.5f);
                UI.Combat.UI.Instance.paused = false;
                return Goals[7];

            case 9:
                UI.Combat.UI.Instance.paused = true;
                return Goals[8];

            case 10:
                return Goals[9];

            case 11:
                return Goals[10];

            case 12:
                _tempUnit.SetHealth(_tempUnit.CurrentHealth * 0.5f);
                UI.Combat.UI.Instance.paused = false;
                return Goals[11];

            case 13:
                UI.Combat.UI.Instance.paused = true;
                return Goals[12];

            case 14:

                _tempUnit.SetHealth(1);
                Florence.RefreshCD();
                Florence.SetHealth(Florence.MaxHealth);
                Florence.SetResource(Florence.GetResourceMax(0), 0);
                Florence.SetResource(Florence.GetResourceMax(1), 1);
                UI.Combat.UI.Instance.paused = false;
                return Goals[13];

            default:
                EndScenario();
                return null;

        }
    }
}