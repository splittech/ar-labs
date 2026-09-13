using System;
using TMPro;
using UnityEngine;

namespace Game.Core
{
    public class FPSCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsTextField;

        /// <summary>
        /// float: time delta in seconds between current frame and last one.
        /// </summary>
        public event Action<float> OnFrameUpdated;

        private void Update()
        {
            OnFrameUpdated?.Invoke(Time.deltaTime);
        }

        public void ShowFPS(float fps)
        {
            _fpsTextField.text = Mathf.Round(fps).ToString();
        }
    }
}