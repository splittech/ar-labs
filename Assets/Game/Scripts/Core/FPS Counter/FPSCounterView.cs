using TMPro;
using UnityEngine;

namespace Game.Core
{
    public class FPSCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsTextField;
        [SerializeField] private float _timeBetweenFPSTextUpdate = 1f;

        public float TimeBetweenFPSTextUpdate => _timeBetweenFPSTextUpdate;

        public void ShowFPS(float fps)
        {
            _fpsTextField.text = Mathf.Round(fps).ToString();
        }
    }
}