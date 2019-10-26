using Fordi.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public class HomeInterface : UIComponent
    {
        public void PlayClick()
        {
            m_gameMachine.LoadGame();
        }

        public override void HelpClick()
        {
            base.HelpClick();
            m_globalUI.Popup(new PopupInfo{
                Content = Lucky7Engine.Lucky7.Lucky7Help,
                Title ="LUCKY 7",
                Preview = null,
                Blocked = true }
            );
        }
    }
}
