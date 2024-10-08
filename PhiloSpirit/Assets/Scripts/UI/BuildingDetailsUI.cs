using Building;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingDetailsUI : MonoBehaviour
    {
        [Header("BuildingUIScreen")]
        [SerializeField] private GameObject _screen;
        [SerializeField] private GameObject _showMenuButton;

        [Header("PlacementDetails")]
        [SerializeField] private BuildingTileDetailsUI _tileDetailsUI;

        [Header("Name")]
        [SerializeField] private Text _buildingName;
        [SerializeField] private Text _costText;
        [SerializeField] private Text _outputText;

        [Header("Cost Prefab")]
        [SerializeField] private ResourceUI _resourceCostPrefab;
        [SerializeField] private SpiritCostUI _spiritCostPrefab;

        [Header("Lists")]
        [SerializeField] private GameObject _costList;
        [SerializeField] private GameObject _outputList;

        [Header("Building")]
        [SerializeField] private BuildingManager _buildingManager;
        [SerializeField] private Button _buildButton;
        [SerializeField] private BuildingUndoManager _undoManager;

        private BuildingData _currentData;

        private void Start()
        {
            Clear();

            _buildingManager.canceledPositioningEvent.AddListener(IndicatorCancelled);
            _undoManager.unselectEvent.AddListener(IndicatorCancelled);
        }

        public void Clear()
        {
            _currentData = null;

            _tileDetailsUI.Clear();

            _buildingName.text = "";
            _costText.gameObject.SetActive(false);
            _outputText.gameObject.SetActive(false);

            ClearList(_costList);
            ClearList(_outputList);

            _buildButton.gameObject.SetActive(false);
        }

        public void ShowDetails(BuildingData data)
        {
            _currentData = data;

            _buildButton.gameObject.SetActive(true);

            _tileDetailsUI.Show(data.tiles);

            _buildingName.text = data.buildingName;
            _costText.gameObject.SetActive(true);
            _outputText.gameObject.SetActive(true);

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

        public void StartBuilding()
        {
            _screen.gameObject.SetActive(false);
            _showMenuButton.SetActive(false);

            _buildingManager.SelectBuilding(_currentData);
        }

        public void StartUndo()
        {
            _screen.gameObject.SetActive(false);
            _showMenuButton.SetActive(false);

            _undoManager.StartUndoSelection();
        }

        private void IndicatorCancelled()
        {
            _screen.gameObject.SetActive(true);
            _showMenuButton.SetActive(true);
        }
    }
}