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
        [TextArea]
        [SerializeField] private string _showModificationValue;
        [TextArea]
        [SerializeField] private string _showDeletionValue;

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

        [Header("ShowStateSpecificText")]
        [SerializeField] private Text _modifyValue;
        [SerializeField] private Text _deleteValue;

        public void Init(TransportState state, bool show, bool deletionPossible)
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
                    if (show)
                    {
                        if (deletionPossible)
                        {
                            _modifyValue.text = "Yes";
                            _deleteValue.text = "Yes";
                        }
                        else
                        {
                            _modifyValue.text = _showModificationValue;
                            _deleteValue.text = _showDeletionValue;
                        }
                    }
                    break;

                case TransportState.Modify:
                    _modifyUI.SetActive(true);
                    InitStateText(_modifyCostText, _modifySpiritText);
                    break;

                default:
                    break;
            }
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