using Terrain;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{

    public class CameraInput : MonoBehaviour
    {
        private Camera _camera;

        private PlayerInput _playerInput;

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        }

        private void OnEnable()
        {
            if (_playerInput != null)
                _playerInput.actions["Select"].performed += SelectTile;
        }

        private void OnDisable()
        {
            if (_playerInput != null)
                _playerInput.actions["Select"].performed -= SelectTile;
        }

        private void SelectTile(InputAction.CallbackContext context)
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit && hit.transform.tag == "Terrain")
            {
                Tile tile = hit.transform.GetComponent<Tile>();
                tile.GetSelected();
            }
        }
    }
}