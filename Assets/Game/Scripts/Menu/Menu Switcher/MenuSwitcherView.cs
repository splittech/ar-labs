using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Game.Menu
{
    public class MenuSwitcherView : MonoBehaviour, IMenuSwitcherView
    {
        [SerializeField] private List<SwitchMenuSetting> _switchMenuSettings;

        public Observable<IMenuView> OnSwitchMenuButtonClicked =>
            _switchMenuSettings
                .SelectMany(setting => setting.SwitchButtonViews
                    .Select(button => button.OnActionPerformed
                        .Where(action => action == ButtonAction.Click)
                        .Select(_ => (IMenuView)setting.MenuView)))
                .Merge();

        [Serializable]
        private class SwitchMenuSetting
        {
            public MenuView MenuView;
            public List<ButtonView> SwitchButtonViews;
        }
    }
}