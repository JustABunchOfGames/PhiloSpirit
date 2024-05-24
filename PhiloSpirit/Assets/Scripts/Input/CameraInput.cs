using Terrain;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{

    public class CameraInput : MonoBehaviour
    {
        private Camera _camera;

        private PlayerInput _playerInput;
        private TileManager _tileManagrer;

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();

            _tileManagrer = GameObject.FindAnyObjectByType<TileManager>();
        }

        private void OnEnable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Select"].performed += SelectTile;
                _playerInput.actions["Unselect"].performed += UnselectTile;
            }
        }

        private void OnDisable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Select"].performed -= SelectTile;
                _playerInput.actions["Unselect"].performed -= UnselectTile;
            }
        }

        private void SelectTile(InputAction.CallbackContext context)
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit && hit.transform.tag == "Terrain")
            {
                _tileManagrer.SetSelectedTile(hit.transform.GetComponent<Tile>());
            }
        }

        private void UnselectTile(InputAction.CallbackContext context)
        {
            _tileManagrer.SetSelectedTile(null);
        }
    }
}