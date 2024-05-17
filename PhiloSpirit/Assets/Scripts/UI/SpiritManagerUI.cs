using Spirits;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class SpiritManagerUI : MonoBehaviour
    {
        [Header("FixedData")]
        [SerializeField] private Image _background;
        [SerializeField] private Text _spiritName;

        [Header("Quantities")]
        [SerializeField] private Text _usableQuantity;
        [SerializeField] private Text _maxQuantity;

        [Header("Costs")]
        [SerializeField] private Text _addCost;
        [SerializeField] private Text _subCost;

        [Header("SubButton to lock it when it's unusable")]
        [SerializeField] private Button _subButton;

        private SpiritManager _spiritManager;
        private SpiritType _spiritType;

        public void Init(SpiritType type, Color backgroundColor, SpiritManager manager)
        {
            _spiritManager = manager;

            _background.color = backgroundColor;

            _spiritName.text = type.ToString();

            _spiritType = type;
        }

        public void UpdateSpirit(SpiritData spiritData)
        {
            _usableQuantity.text = spiritData.usableSpirit.ToString();

            _maxQuantity.text = spiritData.maxSpirit.ToString();

            _addCost.text = "+" + spiritData.addCost.ToString();

            _subCost.text = "-" + spiritData.removeCost.ToString();

            _subButton.interactable = (spiritData.usableSpirit > 0);
        }

        // Called from Button
        public void AddSpirit()
        {
            _spiritManager.AddSpirit(_spiritType);
        }

        // Called from Button
        public void RemoveSpirit()
        {
            _spiritManager.RemoveSpirit(_spiritType);
        }
    }
}