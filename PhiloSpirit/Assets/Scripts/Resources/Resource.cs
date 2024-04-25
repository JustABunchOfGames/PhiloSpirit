using UnityEngine;

namespace Resources
{
    [System.Serializable]
    public enum ResourceType
    {
        Food,
        Wood,
        Stone,
        Metal,
        Ingot,
        Onyx,
        Emerald,
        Sapphire,
        Ruby,
        PilosopherStone
    }

    [System.Serializable]
    public class Resource
    {
        [SerializeField] private ResourceType _type;
        public ResourceType type { get { return _type; } }

        public int quantity;

        public Resource(ResourceType type, int quantity = 0)
        {
            _type = type;
            this.quantity = quantity;
        }
    }
}