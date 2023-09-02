using System;
using UnityEngine;

namespace Core.View.PartyFrame
{
    public class PartyFrame : MonoBehaviour
    {
        [Header("Grid")]
        [Range(1, 20), SerializeField] private byte _raws;
        [Range(1, 20), SerializeField] private byte _columns;

        [Header("Size")]
        [Range(1, 1000), SerializeField] private uint _width;
        [Range(1, 1000), SerializeField] private uint _height;

    }
}
