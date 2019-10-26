using Fordi.Core;
using Fordi.UI.MenuControl;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        void AddPlayer(Player player);
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
        #endregion

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

        public void AddPlayer(Player player)
        {
            foreach (var item in m_playerAnchors)
            {
                if (item.childCount > 0)
                    continue;
                else
                {
                    Instantiate(m_playerViewPrefab, item).DataBind(player);
                    return;
                }
            }
        }
    }
}
