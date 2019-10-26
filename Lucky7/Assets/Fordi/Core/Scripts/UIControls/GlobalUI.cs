using Fordi.Core;
using Fordi.UI.MenuControl;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Fordi.UI
{
    public enum ScreenType
    {
        MAIN_MENU,
        GRID_MENU,
        POPUP,
        NOT_VALID
    }

    public interface IGlobalUI
    {
        bool IsOpen { get; }
        void OpenMenu(MenuItemInfo[] menuItemInfos, bool block = true, bool persist = true);
        void OpenGridMenu(MenuItemInfo[] menuItemInfos, string title, bool block = false, bool persist = true);
        void Popup(PopupInfo popupInfo);
        void CloseLastScreen();
        void LoadHeader();
        Sprite GetRandomAvatar();
        void SwapPlayer(Player player);
    }

    public interface IScreen
    {
        void Reopen();
        void Deactivate();
        void Close();
        bool Blocked { get; }
        bool Persist { get; }
    }

    public class GlobalUI : MonoBehaviour, IGlobalUI
    {
        #region INSPECTOR_REFERENCES
        [SerializeField]
        private MenuScreen m_mainMenuPrefab, m_gridMenuPrefab;
        [SerializeField]
        private Transform m_screensRoot;
        [SerializeField]
        private Popup m_popupPrefab;
        [SerializeField]
        private TextMeshProUGUI m_coinsDisplay;
        [SerializeField]
        private GameObject m_header;
        [SerializeField]
        private GameObject m_uiBlocker;
        [SerializeField]
        private Sprite[] m_avatars;
        [SerializeField]
        private Transform[] m_playerAnchors;
        [SerializeField]
        private PlayerView m_playerViewPrefab;
        [SerializeField]
        private Transform m_playerOrigin;
        #endregion

        private List<PlayerView> m_tablePlayers = new List<PlayerView>();

        private Stack<IScreen> m_screenStack = new Stack<IScreen>();

        public bool IsOpen { get { return m_screenStack.Count != 0; } }

        public void OpenMenu(MenuItemInfo[] items, bool block = true, bool persist = true)
        {
            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                if (screen.Persist)
                    screen.Deactivate();
                else
                    m_screenStack.Pop().Close();
            }

            m_uiBlocker.SetActive(block);
            var menu = Instantiate(m_mainMenuPrefab, m_screensRoot);
            menu.OpenMenu(items, block, persist);
            m_screenStack.Push(menu);
        }

        public void OpenGridMenu(MenuItemInfo[] items, string title, bool block = false, bool persist = true)
        {
            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                if (screen.Persist)
                    screen.Deactivate();
                else
                    m_screenStack.Pop().Close();
            }

            m_uiBlocker.SetActive(block);
            var menu = Instantiate(m_gridMenuPrefab, m_screensRoot);
            menu.OpenGridMenu(items, title, block, persist);
            m_screenStack.Push(menu);
        }

        public void Popup(PopupInfo popupInfo)
        {
            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                if (screen.Persist)
                    screen.Deactivate();
                else
                    m_screenStack.Pop().Close();
            }

            m_uiBlocker.SetActive(popupInfo.Blocked);
            var popup = Instantiate(m_popupPrefab, m_screensRoot);
            popup.Show(popupInfo, null);
            m_screenStack.Push(popup);
        }

        public void CloseLastScreen()
        {
            m_uiBlocker.SetActive(false);

            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Pop();
                screen.Close();
            }

            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                m_uiBlocker.SetActive(screen.Blocked);
                screen.Reopen();
            }
        }

        public void LoadHeader()
        {
            m_header.SetActive(true);
        }

        public Sprite GetRandomAvatar()
        {
            if (m_avatars.Length > 0)
            {
                var index =  UnityEngine.Random.Range(0, m_avatars.Length);
                return m_avatars[index];
            }
            return null;
        }

        public void ClearPlayers()
        {

        }

        public void SwapPlayer(Player player)
        {
            if (m_tablePlayers.Count < 5)
            {
                AddPlayer(player);
                return;
            }

            PlayerView leastTableBidder = m_tablePlayers[0];

            foreach (var item in m_tablePlayers)
                if(item.Player.LastBid < leastTableBidder.Player.LastBid)
                    leastTableBidder = item;

            if (player.LastBid > leastTableBidder.Player.LastBid)
            {
                if (leastTableBidder.Tween != null)
                    leastTableBidder.Tween.Kill();
                m_tablePlayers.Remove(leastTableBidder);
                leastTableBidder.transform.SetParent(leastTableBidder.transform.parent.parent);
                leastTableBidder.transform.DOMove(m_playerOrigin.position, 1f).OnComplete(() => Destroy(leastTableBidder.gameObject));

                AddPlayer(player);
            }
        }

        void AddPlayer(Player player)
        {
            foreach (var item in m_playerAnchors)
            {
                PlayerView playerView = null;
                if (item.childCount > 0)
                    continue;
                else
                {
                    playerView = Instantiate(m_playerViewPrefab, item);
                    Vector3 position = playerView.transform.position;
                    playerView.transform.position = m_playerOrigin.transform.position;
                    playerView.DataBind(player);
                    playerView.Tween = playerView.transform.DOMove(position, 1f);
                    m_tablePlayers.Add(playerView);
                    return;
                    //playerView.transform.DOMove(position, 1.0f).OnComplete(() => playerView.transform.DOMove(m_playerOrigin.position, 1.0f));
                }
            }
        }
    }
}
