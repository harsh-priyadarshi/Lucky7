using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Core;
using Fordi.UI;
using Fordi.UI.MenuControl;
using TMPro;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Fordi.Common;

namespace Fordi.Lucky7Engine
{
    public class Lucky7Interface : UIComponent
    {
        [SerializeField]
        private Timer m_timer;
        [SerializeField]
        private Transform m_bidsPanel;
        [SerializeField]
        private HostView m_hostView;
        [SerializeField]
        private TMP_InputField m_bidAmount;
        [SerializeField]
        private Button m_bidButton;
        [SerializeField]
        private MessageScreen m_lostScreen, m_victoryScreen;

        private Lucky7 m_lucky7;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_globalUI.RemoveOverlay();
                m_bidsPanel.gameObject.SetActive(false);
            }
        }

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

        public void DisplayBidders()
        {
            MenuItemInfo[] menuItems = new MenuItemInfo[m_lucky7.Bidders.Count];

            for(int i = 0; i < m_lucky7.Bidders.Count; i++)
            {
                var item = m_lucky7.Bidders[i];

                MenuItemInfo menuItem = new MenuItemInfo
                {
                    Action = new MenuItemEvent(),
                    Icon = item.Avatar,
                    Text = item.Name
                };
                menuItem.Action.AddListener((args) => item.Display());
                menuItems[i] = menuItem;
            }

            m_globalUI.OpenGridMenu(menuItems, "BIDDERS", true);
        }

        public override void GameUpdate()
        {
            switch (m_lucky7.GameState)
            {
                case GameState.ROUND_BEGAN:
                    m_timer.StartTimer(m_lucky7.Time);
                    m_bidButton.interactable = true;
                    m_bidAmount.interactable = true;
                    break;
                case GameState.WAITING_FOR_RESULT:
                    m_bidButton.interactable = false;
                    m_bidAmount.interactable = false;
                    m_hostView.RollDice((outcome) =>
                    {
                        m_lucky7.DiceRollFinish(outcome);
                    });
                    break;
            }
        }

        public void ClickNumber(int number)
        {
            Bid bid = new Bid
            {
                BidType = BidType.SINGLE,
                BidNumber = number,
                Amount = Convert.ToInt32(m_bidAmount.text)
            };

            m_globalUI.RemoveOverlay();
            m_bidsPanel.gameObject.SetActive(false);

            if (Random.Range(0, 10) < 7)
                m_hostView.Express(Expression.SMILE);
            else
                m_hostView.Express(Expression.BLINK);
            m_lucky7.PlaceBid(bid);
        }

        public void ClickBidPlace()
        {
            m_bidsPanel.gameObject.SetActive(true);
            m_globalUI.Overlay(m_bidsPanel);
            m_hostView.Express(Expression.QUESTION);
        }

        public void ClickBidSlot(int slot)
        {
            Bid bid = new Bid
            {
                BidType = BidType.SLOT,
                Amount = Convert.ToInt32(m_bidAmount.text),
                BidSlot = (BidSlot)slot
            };
            m_globalUI.RemoveOverlay();
            m_bidsPanel.gameObject.SetActive(false);
            if (Random.Range(0, 10) < 4)
                m_hostView.Express(Expression.SMILE);
            else
                m_hostView.Express(Expression.BLINK);
            m_lucky7.PlaceBid(bid);
        }

        public void EnsuerMinimumBidAmount(string value)
        {
            if (string.IsNullOrEmpty(value) || Convert.ToInt32(value) < 100)
                m_bidAmount.text = "" + Lucky7.MinimumBidAmount;
        }

        public void ShowResult(bool won, int amount, Action done)
        {
            MessageScreen screen = null;
            if (won)
            {
                screen = Instantiate(m_victoryScreen);
                screen.Init("Congratulations! You have won Rs. " + amount + ".", () =>
                {
                    m_globalUI.RemoveOverlay();
                    Destroy(screen.gameObject);
                });
            }
            else
            {
                screen = Instantiate(m_lostScreen);
                screen.Init("You lost Rs. " + amount + ".\n Try next time.", () =>
                {
                    m_globalUI.RemoveOverlay();
                    Destroy(screen.gameObject);
                });
            }

            m_globalUI.Overlay(screen.transform);
            m_hostView.CollectDice(done);
            m_globalUI.UpdateCoins(won ? amount : - amount);
        }

        public void CollectDice(Action done)
        {
            m_hostView.CollectDice(done);
        }
    }
}
