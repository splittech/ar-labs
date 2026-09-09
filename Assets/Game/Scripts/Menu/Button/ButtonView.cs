using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public Observable<Unit> OnClick => _button.OnClickAsObservable();
    }
}
