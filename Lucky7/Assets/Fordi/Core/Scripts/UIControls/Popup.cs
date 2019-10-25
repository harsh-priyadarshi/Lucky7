using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fordi.UI.MenuControl;
using System;
using Fordi.Common;

namespace Fordi.UI
{
    public struct PopupInfo
    {
        public string Title;
        public Sprite Preview;
        public string Content;
        public bool Blocked;
    }

    public class Popup : MonoBehaviour, IScreen
    {
        [SerializeField]
        private TextMeshProUGUI m_title, m_text;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private Button m_okButton, m_closeButton;

        public bool Blocked { get; private set; }

        private Action m_closed = null;
        private IGlobalUI m_globalUI;

        private void Awake()
        {
            m_globalUI = IOC.Resolve<IGlobalUI>();
        }

        public void Show(PopupInfo popupInfo, bool blocked, Action closed  = null)
        {
            gameObject.SetActive(true);
            m_closed = closed;

            Blocked = blocked;

            if (!string.IsNullOrEmpty(popupInfo.Title))
                m_title.text = popupInfo.Title;
            else
                m_title.text = "";
            if (!string.IsNullOrEmpty((string)popupInfo.Content))
                m_text.text = (string)popupInfo.Content;
            else
                m_text.text = "";
            if (popupInfo.Preview != null)
            {
                m_icon.sprite = popupInfo.Preview;
                m_icon.transform.parent.gameObject.SetActive(true);
            }
            else
                m_icon.transform.parent.gameObject.SetActive(false);
            if (m_okButton != null)
                m_okButton.onClick.AddListener(() => m_globalUI.CloseLastScreen());
            if (m_closeButton != null)
                m_closeButton.onClick.AddListener(() => m_globalUI.CloseLastScreen());
        }

        public void Close()
        {
            m_closed?.Invoke();
            Destroy(gameObject);
        }

        public void Reopen()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
