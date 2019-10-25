using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Fordi.Core;
using Fordi.Common;

namespace Fordi.UI
{
    [DisallowMultipleComponent]
    public class UIInteractionBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        protected Shadow shadow;

        [SerializeField]
        protected Image selection;

        [SerializeField]
        protected Selectable selectable;

        protected bool pointerHovering = false;

        protected IAppTheme m_appTheme;

        private void Awake()
        {
            m_appTheme = IOC.Resolve<IAppTheme>();
            AwakeOverride();
        }

        private void Start()
        {
            Init();
        }

        protected virtual void AwakeOverride()
        {
            
        }

        public void OnDisable()
        {
            ToggleOutlineHighlight(false);
            ToggleBackgroundHighlight(false);
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);
        }

        public virtual void Init()
        {
            ToggleBackgroundHighlight(false);
            ToggleOutlineHighlight(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerHovering = true;
            ToggleOutlineHighlight(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerHovering = false;
            ToggleOutlineHighlight(false);
            //Debug.LogError("PointerExit");
            ToggleBackgroundHighlight(false);
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ToggleBackgroundHighlight(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ToggleBackgroundHighlight(false);
        }

        public virtual void ToggleOutlineHighlight(bool val)
        {
            if (val && shadow && selectable.interactable)
                shadow.effectColor = m_appTheme.SelectedTheme.baseColor;
            else if(shadow)
                shadow.effectColor = m_appTheme.SelectedTheme.panelInteractionOutline;
        }

        public virtual void ToggleBackgroundHighlight(bool val) { }

        public virtual void OnReset() { }

        public void HighlightOutline(Color col)
        {
            if (shadow)
                shadow.effectColor = col;
        }

        public void HardSelect()
        {
            ToggleBackgroundHighlight(true);
            ToggleOutlineHighlight(true);
        }
    }
}