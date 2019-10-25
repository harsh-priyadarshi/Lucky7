using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fordi.UI.MenuControl;
using System;

namespace Fordi.UI
{
    public class Popup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_title, m_text;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private Button m_okButton, m_closeButton;

        private Action m_closed = null;

        public void Show(PopupInfo popupInfo, Action closed  = null)
        {
            gameObject.SetActive(true);
            m_closed = closed;

            if (!string.IsNullOrEmpty(popupInfo.Title))
                m_title.text = popupInfo.Title;
            else
                m_title.text = "";
            if (!string.IsNullOrEmpty(popupInfo.Content))
                m_text.text = popupInfo.Content;
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
                m_okButton.onClick.AddListener(Close);
            if (m_closeButton != null)
                m_closeButton.onClick.AddListener(Close);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            if (m_okButton != null)
                m_okButton.onClick.RemoveAllListeners();
            if (m_closeButton != null)
                m_closeButton.onClick.RemoveAllListeners();
            m_closed?.Invoke();
            m_closed = null;
        }
    }
}
