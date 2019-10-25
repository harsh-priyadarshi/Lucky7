using Fordi.Core;
using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Lucky7Engine
{
    public class Lucky7 : Game
    {
        public override void Load()
        {
            base.Load();
            m_globalUI.LoadHeader();
        }
    }
}
