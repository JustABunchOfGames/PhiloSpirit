using Building;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButtonUI : MonoBehaviour
    {
        [SerializeField] private BuildingDetailsUI _detailsUI;

        [SerializeField] private BuildingData _data;
        
        [SerializeField] private Text _name;

        private void Start()
        {
            _name.text = _data.buildingName;
        }

        public void ShowDetails()
        {
            _detailsUI.ShowDetails(_data);
        }
    }
}