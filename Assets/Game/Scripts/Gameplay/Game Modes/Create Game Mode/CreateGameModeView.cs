using Game.Menu;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class CreateGameModeView : MenuView
    {
        public enum PudgeTypeButton
        {
            None,
            Normal,
            Happy,
            Sad
        }

        [SerializeField] private RadioButtonView _radioButtonView;
        [SerializeField] private ButtonView _createNormalPudgeButton;
        [SerializeField] private ButtonView _createHappyPudgeButton;
        [SerializeField] private ButtonView _createSadPudgeButton;

        public Subject<PudgeTypeButton> _onPudgeTypeButtonSelected;

        public Observable<PudgeTypeButton> OnPudgeTypeButtonSelected => _radioButtonView.SelectedButton
            .Select(button =>
                {
                    if (button == _createNormalPudgeButton)
                        return PudgeTypeButton.Normal;

                    if (button == _createHappyPudgeButton)
                        return PudgeTypeButton.Happy;

                    if (button == _createSadPudgeButton)
                        return PudgeTypeButton.Sad;

                    return PudgeTypeButton.None;
                });
    }
}
