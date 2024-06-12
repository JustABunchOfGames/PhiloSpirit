using Resources;
using Transport;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class TransportResourceUI : MonoBehaviour
    {
        [SerializeField] private TransportScreenScriptable _scriptable;

        [SerializeField] private Text _name;
        [SerializeField] private Text _quantityText;

        private ResourceType _resourceType;
        private int _quantity;

        public void Init(Resource resource)
        {
            _resourceType = resource.type;
            _quantity = resource.quantity;

            _name.text = _resourceType.ToString();
            _quantityText.text = _quantity.ToString();
        }

        public void Transport(bool transport)
        {
            _scriptable.Transport(_resourceType, transport);
        }

        public bool Substract()
        {
            _quantity--;
            _quantityText.text = _quantity.ToString();

            return _quantity <= 0;
        }

        public void Add()
        {
            _quantity++;
            _quantityText.text = _quantity.ToString();
        }
    }
}