using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fordi.UI
{
    public class MessageScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text;

        [SerializeField]
        private Button m_button;

        public void Init(string text, Action okClick)
        {
            m_text.text = text;
            if (okClick != null)
                m_button.onClick.AddListener(() => okClick.Invoke());
        }
    }
}