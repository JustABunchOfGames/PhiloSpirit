using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class TextUI : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}