using System.Collections.Generic;
using Terrain;
using Transport;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class TileTransportUI : MonoBehaviour
    {
        [Header("Scriptables")]
        [SerializeField] private TransportScriptable _scriptable;
        [SerializeField] private TransportLogger _logger;

        [Header("Costs")]
        [SerializeField] private Text _windSpirit;
        [SerializeField] private Text _totalCost;

        [Header("Lists")]
        [SerializeField] private GameObject _transportToList;
        [SerializeField] private GameObject _transportFromList;

        [Header("Prefab")]
        [SerializeField] private TransportLogUI _logPrefab;

        private Tile _currentTile;

        public void InitTransport(Tile tile)
        {
            _currentTile = tile;

            ClearList(_transportToList);
            ClearList(_transportFromList);

            TransportLogLists logLists = _logger.logDictionary.GetLogs(_currentTile.transform.position);
            if (logLists != null)
            {
                PopulateList(_transportToList, logLists.transportTo, false);
                PopulateList(_transportFromList, logLists.transportFrom, true);

                _windSpirit.text = logLists.windSpiritUsed.ToString();
                _totalCost.text = logLists.totalCost.ToString() + " / " + logLists.possibleCost.ToString();
            }
            else
            {
                _windSpirit.text = "0";
                _totalCost.text = "0";
            }

        }

        private void ClearList(GameObject list)
        {
            bool ignore = true;
            foreach (Transform child in list.transform)
            {
                if (ignore)
                    ignore = false;
                else
                    Destroy(child.gameObject);
            }
        }

        private void PopulateList(GameObject list, List<TransportLog> logList, bool start)
        {
            foreach (TransportLog log in logList)
            {
                TransportLogUI logUI = Instantiate(_logPrefab, list.transform);
                logUI.Init(start ? log.startTileCoord : log.endTileCoord);
            }
        }

        // Called from button
        public void StartTransportCreation(TransportWay way)
        {
            _scriptable.StartTransportCreation(_currentTile, way);
        }
    }
}