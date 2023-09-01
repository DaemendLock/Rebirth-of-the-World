using Core.View.PartyFrame;
using UnityEngine;
using View;

namespace Core.DynamicUI
{
    public class DynamicUIRoot : MonoBehaviour
    {
        [SerializeField] private bool _hide;

        [SerializeField] private ActionBar _actionBar;
        [SerializeField] private PartyFrame _partyFrame;
    }
}
