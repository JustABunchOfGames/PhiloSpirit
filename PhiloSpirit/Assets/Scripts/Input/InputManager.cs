using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        // PlayerInput is out of the scene
        private PlayerInput _playerInput;

        [SerializeField] private Camera _camera;

        [SerializeField] private string _terrainTag;
        [SerializeField] private string _fowTag;

        private bool _canSelect = true;

        public TerrainSelectEvent terrainSelectEvent = new TerrainSelectEvent();
        public TerrainClickedEvent terrainClickedEvent = new TerrainClickedEvent();
        public ScrollEvent scrollEvent = new ScrollEvent();

        private void Awake()
        {
            _playerInput = FindAnyObjectByType<PlayerInput>();
        }

        #region - Enable / Disable -

        private void OnEnable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Select"].performed += Select;
                _playerInput.actions["Unselect"].performed += Unselect;
                _playerInput.actions["Scroll"].performed += Scroll;
            }
        }

        private void OnDisable()
        {
            if (_playerInput != null)
            {
                _playerInput.actions["Select"].performed -= Select;
                _playerInput.actions["Unselect"].performed -= Unselect;
                _playerInput.actions["Scroll"].performed -= Scroll;
            }
        }

        #endregion

        private GameObject GetTerrainWithoutFoW(Ray ray)
        {
            GameObject terrain = null;
            bool _inFogOfWar = false;

            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == _terrainTag)
                    terrain = hit.transform.gameObject;

                if (hit.transform.tag == _fowTag)
                    _inFogOfWar = true;
            }

            if (_inFogOfWar)
                return null;

            return terrain;
        }

        private void Select(InputAction.CallbackContext context)
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (_canSelect)
            {
                GameObject terrain = GetTerrainWithoutFoW(ray);
                if (terrain != null)
                    terrainSelectEvent.Invoke(GetTerrainWithoutFoW(ray));
            }
            else
                terrainClickedEvent.Invoke(GetTerrainWithoutFoW(ray));
        }

        private void Unselect(InputAction.CallbackContext context)
        {
            if (_canSelect)
            {
                terrainSelectEvent.Invoke(null);
            }
            else
            {
                terrainClickedEvent.Invoke(null);
            }
        }

        private void Scroll(InputAction.CallbackContext context)
        {
            float mouseScrollY = context.ReadValue<float>();

            scrollEvent.Invoke(mouseScrollY);
        }

        public void CanSelectTile(bool selecting)
        {
            _canSelect = selecting;
        }

        public GameObject GetHoveredTerrain()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            return GetTerrainWithoutFoW(ray);
        }

        public class TerrainSelectEvent : UnityEvent<GameObject> { }

        public class TerrainClickedEvent : UnityEvent<GameObject> { }

        public class ScrollEvent : UnityEvent<float> { }
    }
}