using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Fordi.Common;
using Fordi.Core;

namespace Fordi.UI
{
    public class VRUIInteractionBase : UIInteractionBase
    {
        public override void ToggleOutlineHighlight(bool val)
        {
        }

        public override void ToggleBackgroundHighlight(bool val)
        {
        }
    }


    public class VRButtonInteraction : VRUIInteractionBase, IPointerClickHandler
    {
        [SerializeField]
        protected TextMeshProUGUI m_text;

        [HideInInspector]
        public Color overriddenColor = Color.white;
        
        [HideInInspector]
        public bool overrideColor = false;

        public override void ToggleOutlineHighlight(bool val)
        {
            if (val && selectable.interactable)
                m_text.color = m_appTheme.SelectedTheme.buttonHighlightTextColor;
            else
                m_text.color = m_appTheme.SelectedTheme.buttonNormalTextColor;

            if (overrideColor)
            {
                selection.color = val ? overriddenColor : Color.white;
            }
        }

        public override void ToggleBackgroundHighlight(bool val) {
          
        }
   
        public override void OnReset()
        {
            //print("Reset");
            if (pointerHovering)
                m_text.color = m_appTheme.SelectedTheme.buttonHighlightTextColor;
            else
                m_text.color = m_appTheme.SelectedTheme.buttonNormalTextColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //print("selectable.interactable: true " + "OnPointerClick");
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (selectable.interactable)
                OnPointerClick(null);
        }

    }
}
