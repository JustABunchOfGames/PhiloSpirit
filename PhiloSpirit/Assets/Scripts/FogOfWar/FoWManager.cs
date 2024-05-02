using UnityEngine;

namespace FogOfWar
{
    public class FoWManager : MonoBehaviour
    {
        [Header("Base position of FoW")]
        [SerializeField] private float _basePos;

        [Header("FoW GameObject")]
        [SerializeField] private Transform _upFoW;
        [SerializeField] private Transform _downFoW;
        [SerializeField] private Transform _leftFoW;
        [SerializeField] private Transform _rightFoW;

        // simple int to know how many times we need to reduce FoW
        private int _tileVisible = 0;

        private void Start()
        {
            ReduceFoW(0);
        }

        public void ReduceFoW(int nbOfTile)
        {
            _tileVisible += nbOfTile;

            float newPos = _basePos + _tileVisible;
            _upFoW.position = new Vector3(0, newPos, 0);
            _downFoW.position = new Vector3(0, -newPos, 0);
            _leftFoW.position = new Vector3(-newPos, 0, 0);
            _rightFoW.position = new Vector3(newPos, 0, 0);
        }
    }
}