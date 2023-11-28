using UnityEngine;
using View.Lobby.Gallery.Widgets;
using View.Lobby.General.Charaters;
using View.Lobby.Utils;

public enum GallerySorting : int
{
    Id = 0,
    Name = 1,
    Level = 2,
    Affection = 3
}

public enum GalleryFilter : int
{
    All = 0,
    Tank = 1,
    Healer = 2,
    Support = 3,
    Dps = 4,
}

namespace View.Lobby.Gallery
{
    public class Gallery : MenuElement
    {
        //private AccountData accountData;
        [SerializeField] private FilesContainerWidget _filesContainer;
        [SerializeField] private CharacterCardWidget _prefab;

        //public void SortWith(int sortingType)
        //{
        //    List<GalleryCard> cur;

        //    switch (sortingType)
        //    {
        //        case 1:
        //            cur = _guildMembers.OrderBy((card) => card.Unit.name).ToList();
        //            break;

        //        case 2:
        //            cur = _guildMembers.OrderBy((card) => card.Unit.lvl).ToList();
        //            break;

        //        case 3:
        //            cur = _guildMembers.OrderBy((card) => card.Unit.affection).ToList();
        //            break;

        //        default:
        //            cur = null; // guildMembers.OrderBy((card) => card.Unit.baseData.UnitId).ToList();
        //            break;
        //    }

        //    for (int i = cur.Count - 1; i > -1; i--)
        //    {
        //        cur[i].gameObject.transform.SetAsFirstSibling();
        //    }
        //}

        //public void FilterWith(int filterType)
        //{
        //    foreach (GalleryCard card in _guildMembers)
        //    {
        //        card.gameObject.SetActive(EvaluateFilted(card, filterType));
        //    }
        //}

        //private bool EvaluateFilted(GalleryCard card, int filter)
        //{
        //    //return filter == (int) GalleryFilter.All || (filter == (int) GalleryFilter.Tank && card.Unit.baseData.Role == UNIT_ROLE.TANK)
        //    //    || (filter == (int) GalleryFilter.Healer && card.Unit.baseData.Role == UNIT_ROLE.HEAL)
        //    //    || (filter == (int) GalleryFilter.Dps && card.Unit.baseData.Role == UNIT_ROLE.DPS);
        //    return false;
        //}

        public void CreateCharacterFile(Character character)
        {
            Object.Instantiate(_prefab, _filesContainer.transform).Init(character);
        }
    }
}