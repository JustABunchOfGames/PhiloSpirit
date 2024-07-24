using Spirits;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "Colors", menuName = "UI/Spirit/Colors")]
    public class SpiritColorScriptable : ScriptableObject
    {
        [SerializeField] private List<SpiritColor> _colorList;

        public Color GetSpiritColor(SpiritType spiritType)
        {
            foreach (SpiritColor spiritColor in _colorList)
            {
                if (spiritColor.type == spiritType)
                    return spiritColor.color;
            }
            return Color.white;
        }
    }

    [System.Serializable]
    public struct SpiritColor
    {
        public SpiritType type;
        public Color color;
    }
}