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
        void OpenMenu(MenuItemInfo[] menuItemInfos, bool block = true);
        void OpenGridMenu(MenuItemInfo[] menuItemInfos, string title, bool block = false);
        void Popup(PopupInfo popupInfo, bool block = false);
        void CloseLastScreen();
        void LoadHeader();
    }

    public interface IScreen
    {
        void Reopen();
        void Deactivate();
        void Close();
        bool Blocked { get; }
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
        #endregion

        private Stack<IScreen> m_screenStack = new Stack<IScreen>();

        public bool IsOpen { get { return m_screenStack.Count != 0; } }

        public void OpenMenu(MenuItemInfo[] items, bool block = true)
        {
            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                screen.Deactivate();
            }
            m_uiBlocker.SetActive(block);
            var menu = Instantiate(m_mainMenuPrefab, m_screensRoot);
            menu.OpenMenu(items, block);
            m_screenStack.Push(menu);
        }

        public void OpenGridMenu(MenuItemInfo[] items, string title, bool block = false)
        {
            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                screen.Deactivate();
            }
            m_uiBlocker.SetActive(block);
            var menu = Instantiate(m_gridMenuPrefab, m_screensRoot);
            menu.OpenGridMenu(items, title, block);
            m_screenStack.Push(menu);
        }

        public void Popup(PopupInfo popupInfo, bool block = false)
        {
            if (m_screenStack.Count > 0)
            {
                var screen = m_screenStack.Peek();
                screen.Deactivate();
            }

            m_uiBlocker.SetActive(popupInfo.Blocked);
            var popup = Instantiate(m_popupPrefab, m_screensRoot);
            popup.Show(popupInfo, block, null);
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
    }
}
