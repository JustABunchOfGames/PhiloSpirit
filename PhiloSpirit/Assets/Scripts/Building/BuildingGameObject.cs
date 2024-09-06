using UnityEngine;

namespace Building
{
    public class BuildingGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject indicator;

        public void ShowIndicator(bool show)
        {
            indicator.SetActive(show);
        }
    }
}