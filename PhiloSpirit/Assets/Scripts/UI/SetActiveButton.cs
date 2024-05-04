using UnityEngine;

namespace UI
{

    public class SetActiveButton : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;

        public void SetActive()
        {
            if (_gameObject != null)
            {
                if (_gameObject.activeSelf)
                {
                    _gameObject.SetActive(false);
                }
                else
                {
                    _gameObject.SetActive(true);
                }
            }
        }
    }
}