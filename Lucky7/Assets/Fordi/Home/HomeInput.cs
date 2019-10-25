using Fordi.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public class HomeInput : InputBase
    {
        public void PlayClick()
        {
            m_gameMachine.LoadGame();
        }

        public override void HelpClick()
        {
            base.HelpClick();
            m_globalUI.Popup(new PopupInfo
            {
                 Content = "Rule 1: Minimum bid amount is Rs. 100.\n\n" +
                 "Rule 2: For Slot 1 to 6 and 8 to 12, bid amount doubles on win.\n\n" +
                 "Rule 3: For slot 7, bid amount tripples on win.",
                 Preview = null,
                 Title = "HELP"
            }, true);
        }
    }
}
