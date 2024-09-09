using Terrain;
using Transport;
using UnityEngine;

namespace UI
{
    public class TransportTutorial : MonoBehaviour
    {
        [Header("Scriptable")]
        [SerializeField] private TransportScriptable _scriptable;

        [Header("TutorialScreen")]
        [SerializeField] private TutorialUI _tutorialUI;
        [SerializeField] private GameObject _transportStart;
        [SerializeField] private GameObject _transportChoose;
        [SerializeField] private GameObject _transportSelection;
        [SerializeField] private GameObject _transportCreation;
        [SerializeField] private GameObject _transportEnd;

        private void OnEnable()
        {
            TileManager.tileChanged.AddListener(ShowTransportChooseTutorial);

            _scriptable.selectionStartEvent.AddListener(ShowTransportSelectionTutorial);
            _scriptable.screenStartEvent.AddListener(ShowTransportCreationTutorial);
            _scriptable.screenConfirmEvent.AddListener(ShowTransportChangeTutorial);
        }

        private void OnDisable()
        {
            TileManager.tileChanged.RemoveListener(ShowTransportChooseTutorial);
            _scriptable.selectionStartEvent.RemoveListener(ShowTransportSelectionTutorial);
            _scriptable.screenStartEvent.RemoveListener(ShowTransportCreationTutorial);
            _scriptable.screenConfirmEvent.RemoveListener(ShowTransportChangeTutorial);
        }

        private void ShowTransportChooseTutorial(Tile tile)
        {
            _transportStart.SetActive(false);
            _transportChoose.SetActive(true);

            TileManager.tileChanged.RemoveListener(ShowTransportChooseTutorial);
        }

        private void ShowTransportSelectionTutorial(Tile tile, TransportWay way)
        {
            _transportChoose.SetActive(false);
            _transportSelection.SetActive(true);

            _scriptable.selectionStartEvent.RemoveListener(ShowTransportSelectionTutorial);
        }

        private void ShowTransportCreationTutorial()
        {
            _transportSelection.SetActive(false);
            _transportCreation.SetActive(true);

            _scriptable.screenStartEvent.RemoveListener(ShowTransportCreationTutorial);
        }

        private void ShowTransportChangeTutorial()
        {
            _tutorialUI.BlockSelection(true);

            _transportCreation.SetActive(false);
            _transportEnd.SetActive(true);

            _scriptable.screenConfirmEvent.RemoveListener(ShowTransportChangeTutorial);
        }
    }
}