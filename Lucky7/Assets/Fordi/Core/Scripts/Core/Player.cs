using Fordi.Common;
using Fordi.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public interface IPlayer
    {
        string Name { get; set; }

        int Money { get; set; }

        Sprite Avatar { get; set; }

        int RoundsPlayed { get; set; }

        int RoundsWon { get; set; }

        void Display();

        void Init(int money, Sprite avatar, int roundsPlayed, int roundsWon);
    }

    public class Player : IPlayer
    {
        public string Name { get; set; }

        public int Money { get; set; }

        public Sprite Avatar { get; set; }

        public int RoundsPlayed { get; set; }

        public int RoundsWon { get; set; }

        private string[] m_playerNames = { "rajesh", "dhananjay", "pradeep", "sachin", "vidya", "parul" };

        private IGlobalUI m_globalUI;

        public Player()
        {
            int index = Random.Range(0, m_playerNames.Length - 1);
            Name = m_playerNames[index] + Random.Range(0, 9) + Random.Range(0, 9);
        }

        public void Init(int money, Sprite avatar, int roundsPlayed, int roundsWon)
        {
            Money = money;
            Avatar = avatar;
            RoundsPlayed = roundsPlayed;
            RoundsWon = roundsWon;
        }

        public void Display()
        {
            if (m_globalUI == null)
                m_globalUI = IOC.Resolve<IGlobalUI>();

            m_globalUI.Popup(new PopupInfo
            {
                Content = "Rounds Won: " + RoundsWon + "/" + RoundsPlayed + "\nMoney: Rs. " + Money + "\nAchievements yet to come.",
                Title = Name,
                Preview = Avatar,
                Blocked = false
            });
        }
    }
}
