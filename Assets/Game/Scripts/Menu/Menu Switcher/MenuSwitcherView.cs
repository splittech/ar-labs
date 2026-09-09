using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Game.Menu
{
    public class MenuSwitcherView : MonoBehaviour
    {
        [SerializeField] private List<SwitchMenuSetting> _switchMenuSettings;

        public Observable<MenuView> OnSwitchMenuButtonClicked =>
            _switchMenuSettings
                .SelectMany(setting => setting.SwitchButtonViews
                    .Select(button => button.OnClick.Select(_ => setting.MenuView)))
                .Merge();

        [Serializable]
        private class SwitchMenuSetting
        {
            public MenuView MenuView;
            public List<ButtonView> SwitchButtonViews;
        }
    }
}