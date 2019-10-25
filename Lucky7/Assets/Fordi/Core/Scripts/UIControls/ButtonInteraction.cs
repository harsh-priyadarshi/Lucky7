using Fordi.Common;
using Fordi.Core;
using Papae.UnitySDK.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fordi.UI
{
    [DisallowMultipleComponent]
    public class ButtonInteraction : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        protected Selectable m_selectable;

        [SerializeField]
        protected TextMeshProUGUI m_text;

        [SerializeField]
        protected IAppTheme m_appTheme;

        private const string ClickClip = "button_click";

        private void Awake()
        {
            m_appTheme = IOC.Resolve<IAppTheme>();
            if (m_selectable == null)
                m_selectable = GetComponent<Selectable>();
            ((Button)m_selectable).onClick.AddListener(() =>
            {
                var clip = AudioManager.Instance.GetClipFromPlaylist(ClickClip);
                AudioManager.Instance.PlayOneShot(clip);
            });            
            AwakeOverride();
        }

        private void OnDestroy()
        {
            OnDestroyOverride();   
        }

        protected virtual void AwakeOverride() { }

        protected virtual void OnDestroyOverride() { }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}