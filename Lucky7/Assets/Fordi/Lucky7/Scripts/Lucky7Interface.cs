using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Core;
using Fordi.UI;
using Fordi.UI.MenuControl;

namespace Fordi.Lucky7Engine
{
    public class Lucky7Interface : UIComponent
    {
        [SerializeField]
        private Timer m_timer;

        private Lucky7 m_lucky7;

        public override void AwakeOverride()
        {
            base.AwakeOverride();
            m_lucky7 = m_gameMachine.GetGame(Playground.LUCKY7).Gameobject.GetComponent<Lucky7>();
            m_lucky7.AddObserver(this);
        }

        public override void OnDestroyOverride()
        {
            base.OnDestroyOverride();
            m_lucky7.RemoveObserver(this);
        }

        public void DisplayPlayer()
        {
            m_player.Display();
        }

        public void DisplayBidders()
        {
            MenuItemInfo[] menuItems = new MenuItemInfo[m_lucky7.Bidders.Count];

            for(int i = 0; i < m_lucky7.Bidders.Count; i++)
            {
                var item = m_lucky7.Bidders[i];

                MenuItemInfo menuItem = new MenuItemInfo
                {
                    Action = new MenuItemEvent(),
                    Icon = item.Avatar,
                    Text = item.Name
                };
                menuItem.Action.AddListener((args) => item.Display());
                menuItems[i] = menuItem;
            }

            m_globalUI.OpenGridMenu(menuItems, "BIDDERS", true);
        }

        public override void GameUpdate()
        {
            m_timer.StartTimer(m_lucky7.Time);
        }
    }
}
