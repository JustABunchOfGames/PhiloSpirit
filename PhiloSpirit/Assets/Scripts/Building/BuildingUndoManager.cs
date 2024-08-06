using Core;
using Input;
using System.Collections;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace Building
{
    public class BuildingUndoManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private TileManager _tileManager;

        private bool _isSelecting;
        private BuildingGameObject _hoveredBuilding;

        public SelectEvent selectEvent = new SelectEvent();
        public UnselectEvent unselectEvent = new UnselectEvent();

        public void StartUndoSelection()
        {
            // Subscribe to used events
            _inputManager.selectEvent.AddListener(Select);
            _inputManager.unselectEvent.AddListener(Unselect);

            // Change behaviour to forbid tile selection
            _tileManager.CanSelect(false);

            _isSelecting = true;
            StartCoroutine(SelectCoroutine());
        }

        private void Select()
        {
            GameObject building = _inputManager.GetHoveredObjectByTag(Tags.buildingTag);
            GameObject terrain = _inputManager.GetHoveredObjectByTag(Tags.terrainTag);

            if (building == null || terrain == null)
                return;

            RemoveListeners();

            _isSelecting = false;

            if (_hoveredBuilding != null)
            {
                _hoveredBuilding.ShowIndicator(false);
                _hoveredBuilding = null;
            }

            // Start Refund building screen
            selectEvent.Invoke(building.GetComponentInParent<BuildingGameObject>(), terrain.GetComponent<Tile>());
        }

        private void Unselect()
        {
            Stop();

            RemoveListeners();

            _isSelecting = false;

            if (_hoveredBuilding != null)
            {
                _hoveredBuilding.ShowIndicator(false);
                _hoveredBuilding = null;
            }

            // Reshow building screen
            unselectEvent.Invoke();
        }

        public void Stop()
        {
            // Change behaviour to allow tile selection
            _tileManager.CanSelect(true);
        }

        private IEnumerator SelectCoroutine()
        {
            _hoveredBuilding = null;
            GameObject newBuilding;
            BuildingGameObject buildingGo;

            while (_isSelecting)
            {
                newBuilding = _inputManager.GetHoveredObjectByTag(Tags.buildingTag);

                if (newBuilding != null)
                {
                    buildingGo = newBuilding.GetComponentInParent<BuildingGameObject>();

                    // if building is changed
                    if (_hoveredBuilding != buildingGo)
                    {
                        if (_hoveredBuilding != null)
                            _hoveredBuilding.ShowIndicator(false);

                        buildingGo.ShowIndicator(true);

                        _hoveredBuilding = buildingGo;
                    }
                }
                else
                {
                    if(_hoveredBuilding != null)
                    {
                        _hoveredBuilding.ShowIndicator(false);

                        _hoveredBuilding = null;
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void RemoveListeners()
        {
            _inputManager.selectEvent.RemoveListener(Select);
            _inputManager.unselectEvent.RemoveListener(Unselect);
        }

        public class SelectEvent : UnityEvent<BuildingGameObject, Tile> { }

        public class UnselectEvent : UnityEvent { }
    }
}