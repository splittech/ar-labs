using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public enum ButtonAction
    {
        Click,
        PointerDown,
        PointerUp
    }

    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;

        [SerializeField] private Color _selectedButtonColor;
        [SerializeField] private Color _deselectedButtonColor;

        public Observable<ButtonAction> OnActionPerformed =>
            Observable.Merge(
                _button.OnClickAsObservable()
                    .Select(_ => ButtonAction.Click),
                _button.OnPointerDownAsObservable()
                    .Select(_ => ButtonAction.PointerDown),
                _button.OnPointerUpAsObservable()
                    .Select(_ => ButtonAction.PointerUp)
            );

        public void Select()
        {
            _buttonImage.color = _selectedButtonColor;
        }

        public void Deselect()
        {
            _buttonImage.color = _deselectedButtonColor;
        }
    }
}
