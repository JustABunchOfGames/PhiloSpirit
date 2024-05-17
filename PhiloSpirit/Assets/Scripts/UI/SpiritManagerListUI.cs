using Spirits;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{

    public class SpiritManagerListUI : MonoBehaviour
    {
        [SerializeField] private SpiritManager _spiritManager;

        [SerializeField] private SpiritColorScriptable _spiritColor;

        [SerializeField] private List<SpiritManagerType> _managerSpiritList;

        private Dictionary<SpiritType, SpiritManagerUI> _managerSpiritDictionaty;

        private void Awake()
        {
            _managerSpiritDictionaty = new Dictionary<SpiritType, SpiritManagerUI>();

            foreach (SpiritManagerType spiritManagerType in _managerSpiritList)
            {
                spiritManagerType.spiritUI.Init(spiritManagerType.spiritType, _spiritColor.GetSpiritColor(spiritManagerType.spiritType), _spiritManager);

                _managerSpiritDictionaty.Add(spiritManagerType.spiritType, spiritManagerType.spiritUI);
            }

            _spiritManager.updateSpiritEvent.AddListener(UpdateSpirit);
        }

        private void UpdateSpirit(SpiritData spiritData, int quantity)
        {
            // We update the usableSpirit quantity, we don't care if we added or substracted a spirit
            _managerSpiritDictionaty[spiritData.type].UpdateSpirit(spiritData);
        }
    }

    [System.Serializable]
    public class SpiritManagerType
    {
        public SpiritType spiritType;
        public SpiritManagerUI spiritUI;
    }
}