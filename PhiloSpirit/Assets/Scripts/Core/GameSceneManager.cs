using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private int _sceneNumber;
            
        private void Start()
        {
            SceneManager.LoadSceneAsync(_sceneNumber, LoadSceneMode.Additive);
        }
    }
}