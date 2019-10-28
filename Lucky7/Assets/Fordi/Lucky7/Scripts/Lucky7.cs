using Fordi.Common;
using Fordi.Core;
using Fordi.UI;
using Fordi.UI.MenuControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fordi.Lucky7Engine
{
    public enum BidType
    {
        SINGLE,
        SLOT
    }

    public enum BidSlot
    {
        CHHOTA_GHAR = 0,
        LUCKY7 = 1,
        BADA_GHAR = 2
    }

    public class Bid
    {
        public BidType BidType;
        public int BidNumber;
        public BidSlot BidSlot;
        public int Amount;
    }

    public class Lucky7 : Game
    {
        public const int MinimumBidAmount = 100;

        public const string Lucky7Help = "Rule 1: Minimum bid amount is Rs. 100.\n\n" +
                 "Rule 2: For Slot 1 to 6 and 8 to 12, bid amount doubles on win.\n\n" +
                 "Rule 3: For slot 7, bid amount tripples on win.";

        private List<Player> m_bidders = new List<Player>();

        public List<Player> Bidders { get { return m_bidders; } }

        private Lucky7Interface m_interface;

        private Bid m_placedBid = null;

        private IEnumerator m_round;

        private int m_maximumBid = 0;

        public int Time { get; protected set; }

        private void StartSimulation(int time)
        {
            m_bidders.Clear();
            m_globalUI.ClearTablePlayers();
            m_placedBid = null;

            m_state = GameState.ROUND_BEGAN;

            if (m_round != null)
                StopCoroutine(m_round);

            Time = time;

            m_round = CoRound();
            StartCoroutine(m_round);

            Notify();
        }

        private IEnumerator CoRound()
        {
            while (Time > 0)
            {
                yield return new WaitForSeconds(1);
                if (Random.Range(0, 10) < 7)
                    CreateNewPlayer();
                if (Random.Range(0, 10) < 4)
                    CreateNewPlayer();
                if (Random.Range(0, 10) == 0)
                    CreateNewPlayer();
                Time--;
            }

            StartDiceRoll();
        }

        private void StartDiceRoll()
        {
            m_state = GameState.WAITING_FOR_RESULT;
            Notify();
        }
       
        void CreateNewPlayer()
        {
            var player = Player.CreateRandomPlayer();
            //Debug.LogError(player.LastBid);
            m_globalUI.SwapPlayer(player);
            m_bidders.Add(player);
            m_bidders.Sort();
        }

        public override void Load()
        {
            base.Load();
            m_globalUI.LoadHeader();
            
            m_player.Init(2500, m_globalUI.GetRandomAvatar(), 0, 0, 0);

            m_globalUI.Popup(new PopupInfo
            {
                Content = Lucky7Help,
                Title = "LUCKY 7",
                Preview = null,
                Blocked = false
            });

            m_interface = FindObjectOfType<Lucky7Interface>();

            StartSimulation(Random.Range(5, 11));
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

        public override void ExecuteButtonCommand(UserInputArgs args)
        {
            if (args == null)
                return;

            base.ExecuteButtonCommand(args);
        }

        public void PlaceBid(Bid bid)
        {
            m_placedBid = bid;
            //Debug.LogError(m_placedBid.BidType.ToString() + " " + m_placedBid.Amount + " " + m_placedBid.BidNumber + " " + m_placedBid.BidSlot.ToString());
        }

        public void DiceRollFinish(int outcome)
        {
            Action action = () => StartSimulation(10);

            if (m_placedBid == null)
            {
                m_interface.CollectDice(action);
                return;
            }

            if (m_placedBid.BidType == BidType.SINGLE)
            {
                if (outcome == m_placedBid.BidNumber)
                    m_interface.ShowResult(true, m_placedBid.Amount * 10, action);
                else
                    m_interface.ShowResult(false, m_placedBid.Amount, action);
            }
            else
            {
                if (outcome < 7 && m_placedBid.BidSlot == BidSlot.CHHOTA_GHAR)
                    m_interface.ShowResult(true, m_placedBid.Amount * 2, action);
                else if (outcome == 7 && m_placedBid.BidSlot == BidSlot.LUCKY7)
                    m_interface.ShowResult(true, m_placedBid.Amount * 3, action);
                else if (outcome > 7 && m_placedBid.BidSlot == BidSlot.BADA_GHAR)
                    m_interface.ShowResult(true, m_placedBid.Amount * 2, action);
                else
                    m_interface.ShowResult(false, m_placedBid.Amount, action);
            }
        }
    }
}
