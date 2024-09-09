using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace Core
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private bool _isLoading; // Just for Editor
        [SerializeField] private int _sceneNumber;
            
        private void Start()
        {
            if (_isLoading)
                SceneManager.LoadSceneAsync(_sceneNumber, LoadSceneMode.Additive);

            // Moving Display to main Display
            MoveWindowAsync(0);
        }

        private async Task MoveWindowTask(int index) // Minimum Unity Version required: 2021.2
        {
            List<DisplayInfo> displayLayout = new List<DisplayInfo>();
            Screen.GetDisplayLayout(displayLayout);
            if (index < displayLayout.Count)
            {
                DisplayInfo display = displayLayout[index];
                Vector2Int position = new Vector2Int(0, 0);
                if (Screen.fullScreenMode != FullScreenMode.Windowed)
                {
                    position.x += display.width / 2;
                    position.y += display.height / 2;
                }
                AsyncOperation asyncOperation = Screen.MoveMainWindowTo(display, position);
                while (asyncOperation.progress < 1f)
                {
                    await Task.Yield();
                }
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private async void MoveWindowAsync(int index)
        {
            await MoveWindowTask(index);
        }
    }
}