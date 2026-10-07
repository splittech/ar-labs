using UnityEngine;

namespace Game.Menu
{
    public class MenuView : MonoBehaviour, IMenuView
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
