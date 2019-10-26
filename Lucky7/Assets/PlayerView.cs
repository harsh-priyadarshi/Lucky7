using Fordi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fordi.UI
{
    public class PlayerView : ButtonInteraction
    {
        private Player m_player;

        [SerializeField]
        private Image m_preview;

        public void DataBind(Player player)
        {
            m_player = player;
            m_preview.sprite = player.Avatar;
            m_text.text = "Rs. " + Random.Range(100, player.Money);
        }

        public void DisplayPlayer()
        {
            m_player.Display();
        }
    }
}