using Building;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingDetailsUI : MonoBehaviour
    {
        [Header("PlacementDetails")]
        [SerializeField] private BuildingTileDetailsUI _tileDetailsUI;

        [Header("Name")]
        [SerializeField] private Text _buildingName;

        [Header("Cost Prefab")]
        [SerializeField] private ResourceUI _resourceCostPrefab;
        [SerializeField] private SpiritCostUI _spiritCostPrefab;

        [Header("Lists")]
        [SerializeField] private GameObject _costList;
        [SerializeField] private GameObject _outputList;

        public void Clear()
        {
            _tileDetailsUI.Clear();

            _buildingName.text = "";

            ClearList(_costList);
            ClearList(_outputList);
        }

        public void ShowDetails(BuildingData data)
        {
            _tileDetailsUI.Show(data.tiles);

            _buildingName.text = data.buildingName;

            ClearList(_costList);
            ClearList(_outputList);

            foreach(SpiritCost cost in data.cost.spiritCost)
            {
                SpiritCostUI costUI = Instantiate(_spiritCostPrefab, _costList.transform);
                costUI.Init(cost.type, cost.quantity);
            }

            foreach(Resource resource in data.cost.resourceCost.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceCostPrefab, _costList.transform);
                resourceUI.Init(resource);
            }

            foreach(Resource resource in data.output.resources)
            {
                ResourceUI resourceUI = Instantiate(_resourceCostPrefab, _outputList.transform);
                resourceUI.Init(resource);
            }
        }

        private void ClearList(GameObject list)
        {
            foreach (Transform child in list.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}