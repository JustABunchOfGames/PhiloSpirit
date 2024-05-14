using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UsableSpiritUI : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _quantity;
        [SerializeField] private Image _background;

        public void Init(string name, int quantity, Color backgroundColor)
        {
            _name.text = name;
            _quantity.text = quantity.ToString();

            _background.color = backgroundColor;
        }

        public void UpdateQuantity(int quantity)
        {
            _quantity.text = quantity.ToString();
        }
    }
}