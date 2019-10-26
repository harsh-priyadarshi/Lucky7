using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Core;
using Fordi.UI;

namespace Fordi.Lucky7Engine
{
    public class Lucky7Interface : UIComponent
    {
        public override void AwakeOverride()
        {
            base.AwakeOverride();
            m_gameMachine.GetGame(Playground.LUCKY7).AddObserver(this);
        }

        public override void OnDestroyOverride()
        {
            base.OnDestroyOverride();
            m_gameMachine.GetGame(Playground.LUCKY7).RemoveObserver(this);
        }

        public void DisplayPlayer()
        {
            m_player.Display();
        }

        public override void GameUpdate()
        {

        }
    }
}
