using Building;
using System;
using Terrain;
using UnityEngine;

namespace UI
{
    public class BuildingTutorial : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private BuildingManager _manager;

        [Header("TutorialScreen")]
        [SerializeField] private GameObject _buildingScreen;
        [SerializeField] private GameObject _buildingPositioning;
        [SerializeField] private GameObject _buildingComplete;

        private void OnEnable()
        {
            _manager.buildingPositioningEvent.AddListener(ShowPositionningTutorial);
            _manager.completeBuildingEvent.AddListener(ShowCompleteBuildingTutorial);
        }

        private void OnDisable()
        {
            _manager.buildingPositioningEvent.RemoveListener(ShowPositionningTutorial);
            _manager.completeBuildingEvent.RemoveListener(ShowCompleteBuildingTutorial);
        }

        private void ShowPositionningTutorial(BuildingData data)
        {
            _buildingScreen.SetActive(false);
            _buildingPositioning.SetActive(true);

            _manager.buildingPositioningEvent.RemoveListener(ShowPositionningTutorial);
        }

        private void ShowCompleteBuildingTutorial(BuildingData data, Tile tile, Quaternion rotation)
        {
            _buildingPositioning.SetActive(false);
            _buildingComplete.SetActive(true);

            _manager.completeBuildingEvent.RemoveListener(ShowCompleteBuildingTutorial);
        }
    }
}