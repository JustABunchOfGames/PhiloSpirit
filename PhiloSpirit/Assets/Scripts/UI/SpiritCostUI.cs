using Spirits;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class SpiritCostUI : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _quantity;

        public void Init(SpiritType spiritType, uint quantity)
        {
            _name.text = spiritType.ToString() + " S.";
            _quantity.text = quantity.ToString();
        }
    }
}