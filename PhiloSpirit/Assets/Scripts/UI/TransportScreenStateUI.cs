using Transport;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TransportScreenStateUI : MonoBehaviour
    {
        [Header("Create")]
        [SerializeField] private GameObject _createUI;
        [TextArea]
        [SerializeField] private string _createCostText;
        [TextArea]
        [SerializeField] private string _createSpiritText;

        [Header("Show")]
        [SerializeField] private GameObject _showUI;
        [TextArea]
        [SerializeField] private string _showCostText;
        [TextArea]
        [SerializeField] private string _showSpiritText;

        [Header("Modify")]
        [SerializeField] private GameObject _modifyUI;
        [TextArea]
        [SerializeField] private string _modifyCostText;
        [TextArea]
        [SerializeField] private string _modifySpiritText;

        [Header("Text")]
        [SerializeField] private Text _costText;
        [SerializeField] private Text _spiritText;
        [SerializeField] private Text _costQuantity;
        [SerializeField] private Text _spiritQuantity;

        public void Init(TransportState state, string cost, string spirit)
        {
            _createUI.SetActive(false);
            _showUI.SetActive(false);
            _modifyUI.SetActive(false);

            switch (state)
            {

                case TransportState.Create:
                    _createUI.SetActive(true);
                    InitStateText(_createCostText, _createSpiritText);
                    break;

                case TransportState.Show:
                    _showUI.SetActive(true);
                    InitStateText(_showCostText, _showSpiritText);
                    break;

                case TransportState.Modify:
                    _modifyUI.SetActive(true);
                    InitStateText(_modifyCostText, _modifySpiritText);
                    
                    break;

                default:
                    break;
            }

            UpdateText(cost, spirit);
        }

        private void InitStateText(string costString, string spiritString)
        {

            _costText.text = costString;
            _spiritText.text = spiritString;
        }

        public void UpdateText(string cost, string spirit)
        {
            _costQuantity.text = cost;
            _spiritQuantity.text = spirit;
        }
    }
}