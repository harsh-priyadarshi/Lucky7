using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Core;
using Fordi.UI;

namespace Fordi.Lucky7Engine
{
    public class Lucky7Interface : UIComponent
    {
        [SerializeField]
        private Timer m_timer;

        [SerializeField]
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

        public override void GameUpdate()
        {
            m_timer.StartTimer(m_lucky7.Time);
        }
    }
}
