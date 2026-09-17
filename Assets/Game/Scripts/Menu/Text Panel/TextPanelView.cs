using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class TextPanelView : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> _textFields;

        public List<TMP_Text> TextFields => _textFields;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateText(int textIndex, string newText)
        {
            _textFields[textIndex].text = newText;

            // Layout does not update automatically when we set text via script.
            LayoutRebuilder.ForceRebuildLayoutImmediate(_textFields[textIndex].rectTransform);
        }
    }
}