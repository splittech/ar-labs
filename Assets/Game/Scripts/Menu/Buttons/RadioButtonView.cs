using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Game.Menu
{
    public class RadioButtonView : MonoBehaviour
    {
        [SerializeField] private List<ButtonView> _buttonViews;

        private readonly ReactiveProperty<ButtonView> _selectedButton = new(null);

        public ReadOnlyReactiveProperty<ButtonView> SelectedButton => _selectedButton;

        private void Awake()
        {
            _selectedButton.AddTo(this);

            foreach (var button in _buttonViews)
            {
                button.OnActionPerformed
                    .Subscribe(_ => OnButtonClick(button))
                    .AddTo(this);
            }
        }

        private void OnButtonClick(ButtonView button)
        {
            ButtonView previousButton = _selectedButton.Value;
            ButtonView nextButton = button == previousButton ? null : button;

            if (previousButton != null)
                previousButton.Deselect();

            if (nextButton != null)
                nextButton.Select();

            _selectedButton.Value = nextButton;
        }
    }
}