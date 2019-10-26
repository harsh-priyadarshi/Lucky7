using Fordi.Common;
using Fordi.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

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

        void Init(int money, Sprite avatar, int roundsPlayed, int roundsWon, int lastBid);
    }

    public class Player : IPlayer, IComparable<Player>
    {
        public string Name { get; set; }

        public int Money { get; set; }

        public Sprite Avatar { get; set; }

        public int RoundsPlayed { get; set; }

        public int RoundsWon { get; set; }

        public int LastBid { get; set; }

        private string[] m_playerNames = { "rajesh", "dhananjay", "pradeep", "sachin", "vidya", "parul" };

        private static IGlobalUI m_globalUI;

        public Player()
        {
            int index = UnityRandom.Range(0, m_playerNames.Length - 1);
            Name = m_playerNames[index] + UnityRandom.Range(0, 9) + UnityRandom.Range(0, 9);
        }

        public static Player CreateRandomPlayer()
        {
            if (m_globalUI == null)
                m_globalUI = IOC.Resolve<IGlobalUI>();

            var player = new Player();
            var totalRounds = UnityRandom.Range(0, 40);
            var money = UnityRandom.Range(100, 40000);
            var bidAmount = UnityRandom.Range(100, money);
            player.Init(UnityRandom.Range(0, 40000), m_globalUI.GetRandomAvatar(), totalRounds, UnityRandom.Range(0, totalRounds/2), bidAmount);
            return player;
        }

        public void Init(int money, Sprite avatar, int roundsPlayed, int roundsWon, int lastBid)
        {
            Money = money;
            Avatar = avatar;
            RoundsPlayed = roundsPlayed;
            RoundsWon = roundsWon;
            LastBid = lastBid;
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

        public int CompareTo(Player other)
        {
            if (other == null)
               return LastBid;

            return LastBid - other.LastBid;
        }
    }
}
