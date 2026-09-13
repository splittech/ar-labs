using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    }
}