using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class ResourceUI : MonoBehaviour
    {
        [SerializeField] protected Text _name;
        [SerializeField] protected Text _quantity;

        public void Init(Resource resource)
        {
            _name.text = resource.type.ToString();
            _quantity.text = resource.quantity.ToString();
        }

        public void Init(Resource taskResource, Resource portalResource)
        {
            _name.text = taskResource.type.ToString();
            _quantity.text = portalResource.quantity.ToString() + " / " + taskResource.quantity.ToString();
        }
    }
}