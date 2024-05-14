using Spirits;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{

    [CreateAssetMenu(fileName = "UI/Spirit/Colors")]
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
    public class SpiritColor
    {
        public SpiritType type;
        public Color color;
    }

    [System.Serializable]
    public class SerializableColor
    {

        public float[] colorStore = new float[4] { 1F, 1F, 1F, 1F };
        public Color Color
        {
            get { return new Color(colorStore[0], colorStore[1], colorStore[2], colorStore[3]); }
            set { colorStore = new float[4] { value.r, value.g, value.b, value.a }; }
        }

        //makes this class usable as Color, Color normalColor = mySerializableColor;
        public static implicit operator Color(SerializableColor instance)
        {
            return instance.Color;
        }

        //makes this class assignable by Color, SerializableColor myColor = Color.white;
        public static implicit operator SerializableColor(Color color)
        {
            return new SerializableColor { Color = color };
        }
    }
}