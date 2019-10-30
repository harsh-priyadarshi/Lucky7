using Fordi.Common;
using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fordi.UI
{
    public class MenuScreen : MonoBehaviour, IScreen
    {
        [SerializeField]
        protected Transform m_contentRoot;

        [SerializeField]
        protected MenuItem m_menuItem;

        [SerializeField]
        private TextMeshProUGUI m_title;

        [SerializeField]
        private Button m_closeButton, m_okButton;

        private IGlobalUI m_globalUI;

        public bool Blocked { get; private set; }

        public bool Persist { get; private set; }

        public GameObject Gameobject { get { return gameObject; } }

        void Awake()
        {
            m_globalUI = IOC.Resolve<IGlobalUI>();
        }


        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Reopen()
        {
            gameObject.SetActive(true);
        }


        public void SpawnMenuItem(MenuItemInfo menuItemInfo, MenuItem prefab, Transform parent)
        {
            MenuItem menuItem = Instantiate(prefab, parent, false);
            menuItem.name = "MenuItem";
            menuItem.Item = menuItemInfo;
        }

        public void Clear()
        {
            foreach (Transform child in m_contentRoot)
            {
                Destroy(child.gameObject);
            }
            m_contentRoot.DetachChildren();
        }

        public void OpenMenu(MenuItemInfo[] items, bool blocked, bool persist)
        {
            Clear();
            Blocked = blocked;
            Persist = persist;
            gameObject.SetActive(true);
            foreach (var item in items)
                SpawnMenuItem(item, m_menuItem, m_contentRoot);

            if (m_globalUI == null)
                m_globalUI = IOC.Resolve<IGlobalUI>();

            if (m_okButton != null)
                m_okButton.onClick.AddListener(() => m_globalUI.CloseLastScreen());
            if (m_closeButton != null)
                m_closeButton.onClick.AddListener(() => m_globalUI.CloseLastScreen());
        }

        public void OpenGridMenu(MenuItemInfo[] items, string title, bool blocked, bool persist)
        {
            if (m_title != null)
                m_title.text = title;
            OpenMenu(items, blocked, persist);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}