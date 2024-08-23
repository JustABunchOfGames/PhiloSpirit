using Resources;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TaskResourceUI : ResourceUI
    {
        [SerializeField] private TextMeshProUGUI _removeText;

        private int _quantityValue;

        public new void Init(Resource resource)
        {
            base.Init(resource);

            _quantityValue = resource.quantity;
        }

        public void RemoveQuantity(int quantityToRemove)
        {
            // Instantiate a red text to show reduction
            TextMeshProUGUI removeText = Instantiate(_removeText, transform);
            removeText.text = "-" + quantityToRemove.ToString();

            // Update text
            _quantityValue -= quantityToRemove;
            _quantity.text = _quantityValue.ToString();
        }
    }
}