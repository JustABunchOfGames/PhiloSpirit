using Terrain;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{

    public class InputManager : MonoBehaviour
    {
        // PlayerInput is out of the scene
        private PlayerInput _playerInput;

        [SerializeField] private Camera _camera;
        [SerializeField] private TileManager _tileManagrer;

        private bool _isSelecting = true;
        public TileClickedEvent tileClickedEvent = new TileClickedEvent();

        private void Awake()
        {
            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        }

        private void OnEnable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Select"].performed += Select;
                _playerInput.actions["Unselect"].performed += Unselect;
            }
        }

        private void OnDisable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Select"].performed -= Select;
                _playerInput.actions["Unselect"].performed -= Unselect;
            }
        }

        private void Select(InputAction.CallbackContext context)
        {

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit && hit.transform.tag == "Terrain")
            {
                if (_isSelecting)
                {
                    _tileManagrer.SetSelectedTile(hit.transform.GetComponent<Tile>());
                }
                else
                {
                    tileClickedEvent.Invoke(hit.transform.GetComponent<Tile>());
                }
            }
        }

        private void Unselect(InputAction.CallbackContext context)
        {
            if (_isSelecting)
            {
                _tileManagrer.SetSelectedTile(null);
            }
            else
            {
                tileClickedEvent.Invoke(null);
            }
        }

        public void IsSelecting(bool selecting)
        {
            _isSelecting = selecting;
        }

        public Tile GetHoveredTile()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit && hit.transform.tag == "Terrain")
            {
                return hit.transform.GetComponent<Tile>();
            }

            return null;
        }

        public class TileClickedEvent : UnityEvent<Tile> { }
    }
}