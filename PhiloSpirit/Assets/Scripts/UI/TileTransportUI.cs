using System.Collections.Generic;
using Terrain;
using Transport;
using UnityEngine;

namespace UI
{

    public class TileTransportUI : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private TransportManager _transportManager;

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

            TransportLogLists logLists = _transportManager.logDictionary.GetLogs(_currentTile.transform.position);
            if (logLists != null)
            {
                PopulateList(_transportToList, logLists.transportTo, false);
                PopulateList(_transportFromList, logLists.transportFrom, true);
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
        public void StartTransport(bool startToEnd)
        {
            _transportManager.StartTransport(_currentTile, startToEnd);
        }
    }
}