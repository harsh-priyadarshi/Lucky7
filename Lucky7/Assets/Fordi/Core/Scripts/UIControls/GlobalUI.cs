using Fordi.UI.MenuControl;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fordi.UI
{
    public struct PopupInfo
    {
        public string Title;
        public string Content;
        public Sprite Preview;
    }

    public interface IGlobalUI
    {
        void Clear();
        void OpenMenu(MenuItemInfo[] menuItemInfos, bool block = true);
        void CloseMenu();
        void OpenGridMenu(MenuItemInfo[] menuItemInfos, string title, bool block = false);
        void LoadHeader();
        bool IsMenuOpen { get; }
        void Popup(PopupInfo popupInfo, bool block = false);
    }

    public class GlobalUI : MonoBehaviour, IGlobalUI
    {
        [Header("Menu")]
        [SerializeField]
        private RectTransform m_menuContentRoot;
        [SerializeField]
        private RectTransform m_gridContentRoot;
        [SerializeField]
        private GameObject m_menuItem;
        [SerializeField]
        private GameObject m_gridMenuItem;
        [SerializeField]
        private RectTransform m_mainMenuRoot, m_menuRoot;
        [SerializeField]
        private RectTransform m_menu, m_gridMenu;
        [SerializeField]
        private TextMeshProUGUI m_gridWindowTitle;

        [Header("Others")]
        [SerializeField]
        private Popup m_popup;
        [SerializeField]
        private TextMeshProUGUI m_coinsDisplay;
        [SerializeField]
        private GameObject m_header;
        [SerializeField]
        private GameObject m_uiBlocker;

        private GameObject m_menuInstance;

        private bool m_isMenuOpen = false;

        public bool IsMenuOpen { get { return m_isMenuOpen; } }

        public void SpawnMenuItem(MenuItemInfo menuItemInfo, GameObject prefab, Transform parent)
        {
            MenuItem menuItem = Instantiate(prefab, parent, false).GetComponentInChildren<MenuItem>();
            menuItem.name = "MenuItem";
            menuItem.Item = menuItemInfo;
        }

        public void Clear()
        {
            foreach (Transform child in m_menuContentRoot)
            {
                Destroy(child.gameObject);
            }
            m_menuContentRoot.DetachChildren();

            foreach (Transform child in m_gridContentRoot)
            {
                Destroy(child.gameObject);
            }
            m_gridContentRoot.DetachChildren();
        }

        public void OpenMenu(MenuItemInfo[] menuItemInfos, bool block = true)
        {
            Clear();
            m_uiBlocker.SetActive(block);
            m_mainMenuRoot.gameObject.SetActive(true);
            m_menu.gameObject.SetActive(true);
            m_gridMenu.gameObject.SetActive(false);
            foreach (var item in menuItemInfos)
                SpawnMenuItem(item, m_menuItem, m_menuContentRoot);
            m_isMenuOpen = true;
        }

        public void OpenGridMenu(MenuItemInfo[] menuItemInfos, string windowTitle, bool block = false)
        {
            Clear();
            m_uiBlocker.SetActive(block);
            m_mainMenuRoot.gameObject.SetActive(true);
            m_menu.gameObject.SetActive(false);
            m_gridMenu.gameObject.SetActive(true);
            //m_gridWindowTitle.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(windowTitle));
            m_gridWindowTitle.text = windowTitle;
            foreach (var item in menuItemInfos)
                SpawnMenuItem(item, m_gridMenuItem, m_gridContentRoot);
            m_isMenuOpen = true;
        }

        public void CloseMenu()
        {
            m_mainMenuRoot.gameObject.SetActive(false);
            m_gridMenu.gameObject.SetActive(false);
            m_menu.gameObject.SetActive(false);
            m_isMenuOpen = false;
            if (m_menuInstance != null)
                Destroy(m_menuInstance);
            m_uiBlocker.SetActive(false);
        }

        public void LoadHeader()
        {
            m_header.SetActive(true);
        }

        public void Popup(PopupInfo popupInfo, bool block = false)
        {
            m_uiBlocker.SetActive(block);
            m_popup.Show(popupInfo, () => m_uiBlocker.SetActive(false));
        }
    }
}
