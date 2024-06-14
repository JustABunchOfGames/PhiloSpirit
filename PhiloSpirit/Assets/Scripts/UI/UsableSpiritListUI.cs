using Spirits;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public class UsableSpiritListUI : MonoBehaviour
    {
        [SerializeField] private SpiritColorScriptable _spiritColor;

        [SerializeField] private List<UsableSpiritType> _usableSpiritList;

        private Dictionary<SpiritType, UsableSpiritUI> _usableSpiritDictionaty;

        private void Awake()
        {
            _usableSpiritDictionaty = new Dictionary<SpiritType, UsableSpiritUI>();

            foreach(UsableSpiritType usableSpiritType in _usableSpiritList)
            {
                usableSpiritType.spiritUI.Init(usableSpiritType.spiritType.ToString(), 0, _spiritColor.GetSpiritColor(usableSpiritType.spiritType));

                _usableSpiritDictionaty.Add(usableSpiritType.spiritType, usableSpiritType.spiritUI);
            }

            SpiritManager.updateSpiritEvent.AddListener(UpdateSpirit);
        }

        private void UpdateSpirit(SpiritData spiritData, int quantity)
        {
            // We update the usableSpirit quantity, we don't care if we added or substracted a spirit
            _usableSpiritDictionaty[spiritData.type].UpdateQuantity(spiritData.usableSpirit);
        }
    }

    [System.Serializable]
    public class UsableSpiritType
    {
        public SpiritType spiritType;
        public UsableSpiritUI spiritUI;
    }
}