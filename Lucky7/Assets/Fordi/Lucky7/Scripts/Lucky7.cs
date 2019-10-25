using Fordi.Core;
using Fordi.UI;
using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Lucky7Engine
{
    public class Lucky7 : Game
    {
        public const string Lucky7Help = "Rule 1: Minimum bid amount is Rs. 100.\n\n" +
                 "Rule 2: For Slot 1 to 6 and 8 to 12, bid amount doubles on win.\n\n" +
                 "Rule 3: For slot 7, bid amount tripples on win.";

        public override void Load()
        {
            base.Load();
            m_globalUI.LoadHeader();
        }

        public override void ExecuteMenuCommand(MenuClickArgs args)
        {
            if (args == null)
                return;
            base.ExecuteMenuCommand(args);
            switch (args.Command)
            {
                case "back":
                    m_globalUI.CloseLastScreen();
                    break;
                case "settings":
                    break;
                case "help":
                    m_globalUI.Popup(new PopupInfo
                    {
                        Content = Lucky7Help,
                        Title = "LUCKY 7",
                        Preview = null,
                        Blocked = true
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
