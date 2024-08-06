using Core;
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

        public SelectEvent selectEvent = new SelectEvent();
        public UnselectEvent unselectEvent = new UnselectEvent();
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

        private GameObject GetObjectByTag(Ray ray, string tag)
        {
            GameObject go = null;
            bool _isGettable = true;

            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == tag)
                    go = hit.transform.gameObject;

                if (hit.transform.tag == Tags.fogOfWarTag)
                    _isGettable = false;
            }

            if (!_isGettable)
                return null;

            return go;
        }

        private void Select(InputAction.CallbackContext context)
        {
            selectEvent.Invoke();
        }

        private void Unselect(InputAction.CallbackContext context)
        {
            unselectEvent.Invoke();
        }

        private void Scroll(InputAction.CallbackContext context)
        {
            float mouseScrollY = context.ReadValue<float>();

            scrollEvent.Invoke(mouseScrollY);
        }

        public GameObject GetHoveredObjectByTag(string tag)
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            return GetObjectByTag(ray, tag);
        }

        public class SelectEvent : UnityEvent { }
        public class UnselectEvent : UnityEvent { }

        public class ScrollEvent : UnityEvent<float> { }
    }
}