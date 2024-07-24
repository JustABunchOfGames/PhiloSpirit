using Building;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingTileDetailsUI : MonoBehaviour
    {
        [SerializeField] private Image _tileImagePrefab;
        [SerializeField] private Color _hideColor;

        [SerializeField] private TerrainColorScriptable _terrainColors;

        private Vector3 _startPosition;
        [SerializeField] private float _xOffset;
        [SerializeField] private float _yOffset;

        private Dictionary<Vector3, Image> _tiles;

        private void Start()
        {
            _tiles = new Dictionary<Vector3, Image>();

            _startPosition = transform.position;
            
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    Image image = Instantiate(_tileImagePrefab, 
                        new Vector3(_startPosition.x + i * _xOffset, _startPosition.y + j * _yOffset),
                        Quaternion.identity, transform);

                    image.color = _hideColor;

                    _tiles.Add(new Vector3(i, j, 0), image);
                }
            }
            
        }

        public void Clear()
        {
            foreach (Image tile in _tiles.Values)
            {
                tile.color = _hideColor;
            }
        }

        public void Show(List<BuildingTile> buildingTiles)
        {
            Clear();

            foreach(BuildingTile tile in buildingTiles)
            {
                _tiles[tile.coord].color = _terrainColors.GetTerrainColor(tile.neededTileType);
            }
        }
    }
}