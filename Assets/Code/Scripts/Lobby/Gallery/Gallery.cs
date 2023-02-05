using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GallerySorting : int {
    Id = 0,
    Name = 1,
    Level = 2,
    Affection = 3
}

public enum GalleryFilter : int {
    All = 0,
    Tank = 1,
    Healer = 2,
    Dps = 3
}

public class Gallery : MonoBehaviour {
    private AccountData accountData;
    public List<GalleryCard> guildMembers;

    private void OnEnable() {
        accountData = Lobby.ActiveAccount.data;
       foreach(UnitPreview unit in accountData.OwnerCharacters) {
            guildMembers[unit.baseData.unitId].SetUnit(unit) ;
       }
    }

    public void SortWith(int sortingType) {
        List<GalleryCard> cur;
        switch (sortingType) {
            case 1:
                cur = guildMembers.OrderBy((card) => card.Unit.name).ToList();
                break;
            case 2:
                cur = guildMembers.OrderBy((card) => card.Unit.lvl).ToList();
                break;
            case 3:
                cur = guildMembers.OrderBy((card) => card.Unit.affection).ToList();
                break;
            default:
                cur = guildMembers.OrderBy((card) => card.Unit.baseData.unitId).ToList();
                break;
        }
        for (int i = cur.Count - 1; i > -1; i--) {
            cur[i].gameObject.transform.SetAsFirstSibling();
        }
    }

    public void FilterWith(int filterType) {
        foreach (GalleryCard card in guildMembers) {
            card.gameObject.SetActive( EvaluateFilted(card, filterType));
        }
    }

    private bool EvaluateFilted(GalleryCard card, int filter) {
        return filter == (int)GalleryFilter.All || (filter == (int)GalleryFilter.Tank && card.Unit.baseData.role == UNIT_ROLE.TANK) 
            || (filter == (int)GalleryFilter.Healer && card.Unit.baseData.role == UNIT_ROLE.HEAL)
            || (filter == (int)GalleryFilter.Dps && card.Unit.baseData.role == UNIT_ROLE.DPS);
    }
}
