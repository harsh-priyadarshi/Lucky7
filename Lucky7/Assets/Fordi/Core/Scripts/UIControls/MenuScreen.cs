using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        public bool Blocked { get; private set; }

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

        public void OpenMenu(MenuItemInfo[] items, bool blocked)
        {
            Clear();
            Blocked = blocked;
            gameObject.SetActive(true);
            foreach (var item in items)
                SpawnMenuItem(item, m_menuItem, m_contentRoot);
        }

        public void OpenGridMenu(MenuItemInfo[] items, string title, bool blocked)
        {
            if (m_title != null)
                m_title.text = title;
            OpenMenu(items, blocked);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}