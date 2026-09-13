using System.Collections.Generic;
using Game.Menu;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class EditGameModeView : MenuView
    {
        public enum Button
        {
            SwitchDescription,
            AddScale,
            SubstractScale,
            RotateClockwise,
            RotateCounterClockwise
        }

        [Header("Information Panel")]
        [SerializeField] private TextPanelView _nameTextPanel;
        [SerializeField] private TextPanelView _desciptionTextPanel;
        [SerializeField] private TextPanelView _transformationTextPanel;

        [Header("Buttons")]
        [SerializeField] private ButtonView _switchDescriptionButton;
        [SerializeField] private ButtonView _addScaleButton;
        [SerializeField] private ButtonView _substractSacaleButton;
        [SerializeField] private ButtonView _rotateClockwiseButton;
        [SerializeField] private ButtonView _rotateCounterClockwiseButton;

        private List<TextPanelView> _allTextPanels = new();
        private int _currentTextFieldIndex;

        public Observable<Button> OnButtonPressed => Observable.Merge(
            ObserveClick(_switchDescriptionButton, Button.SwitchDescription),
            ObserveClick(_addScaleButton, Button.AddScale),
            ObserveClick(_substractSacaleButton, Button.SubstractScale),
            ObserveClick(_rotateClockwiseButton, Button.RotateClockwise),
            ObserveClick(_rotateCounterClockwiseButton, Button.RotateCounterClockwise)
        );

        private static Observable<Button> ObserveClick(ButtonView buttonView, Button button)
        {
            return buttonView.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Select(_ => button);
        }

        private void Awake()
        {
            _allTextPanels.AddRange(new List<TextPanelView>
            {
                _nameTextPanel, _desciptionTextPanel, _transformationTextPanel
            });

            _currentTextFieldIndex = 0;
            ShowCurrentTextField();

            _switchDescriptionButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => ShowNextTextField())
                .AddTo(this);
        }

        public void UpdateTextPanels(Pudge selectedPudge)
        {
            if (selectedPudge == null)
                return;

            _nameTextPanel.TextFields[0].text = selectedPudge.Name;
            _desciptionTextPanel.TextFields[0].text = selectedPudge.Description;
            _transformationTextPanel.TextFields[0].text = "0";
            _transformationTextPanel.TextFields[1].text = "0";
        }

        public void UpdateScaleText(float scale)
        {
            _transformationTextPanel.TextFields[0].text = scale.ToString();
        }

        public void UpdateAngleText(float angle)
        {
            _transformationTextPanel.TextFields[1].text = angle.ToString();
        }

        private void ShowNextTextField()
        {
            _currentTextFieldIndex++;
            ShowCurrentTextField();
        }

        private void ShowCurrentTextField()
        {
            _allTextPanels.ForEach(textField => textField.gameObject.SetActive(false));
            _allTextPanels[_currentTextFieldIndex % _allTextPanels.Count].Show();
        }
    }
}
