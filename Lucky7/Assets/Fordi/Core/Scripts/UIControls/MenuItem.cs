using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Fordi.Common;
using Fordi.Core;

namespace Fordi.UI.MenuControl
{
    public delegate void MenuItemEventHandler(MenuItem menuItem);

    public class MenuItem : ButtonInteraction
    {
        [SerializeField]
        private Image m_icon = null;

        private IGameMachine m_experienceMachine;

        private MenuItemInfo m_item;
        public MenuItemInfo Item
        {
            get { return m_item; }
            set
            {
                if(m_item != value)
                {
                    m_item = value;
                    DataBind();
                }
            }
        }

        private void DataBind()
        {
            if(m_item != null)
            {
                m_icon.sprite = m_item.Icon;
                if (m_item.Data != null && m_item.Data is Color)
                    m_icon.color = (Color)m_item.Data;
                m_icon.gameObject.SetActive(m_item.Icon != null || (m_item.Data != null && m_item.Data is Color));
                m_text.text = m_item.Text;
            }
            else
            {
                m_icon.sprite = null;
                m_icon.gameObject.SetActive(false);
                m_text.text = string.Empty;
            }

            m_item.Validate = new MenuItemValidationEvent();

            if (m_experienceMachine == null)
                m_experienceMachine = IOC.Resolve<IGameMachine>();
            if (m_appTheme == null)
                m_appTheme = IOC.Resolve<IAppTheme>();

            m_item.Validate.AddListener(m_experienceMachine.CanExecuteMenuCommand);
            m_item.Validate.AddListener((args) => args.IsValid = m_item.IsValid);

            var validationResult = IsValid();
            if(validationResult.IsVisible)
            {
                if (m_item.IsValid)
                {
                    m_text.color = m_appTheme.SelectedTheme.buttonNormalTextColor;
                }
                else
                {
                    m_text.color = m_appTheme.SelectedTheme.buttonDisabledTextColor;
                }
                m_item.Action = new MenuItemEvent();
                m_item.Action.AddListener(m_experienceMachine.ExecuteMenuCommand);
                ((Button)m_selectable).onClick.AddListener(() => m_item.Action.Invoke(new MenuClickArgs(m_item.Path, m_item.Text, m_item.Command, m_item.CommandType, m_item.Data)));
            }

            gameObject.SetActive(validationResult.IsVisible);
            m_selectable.interactable = validationResult.IsValid;
        }

        private MenuItemValidationArgs IsValid()
        {
            if(m_item == null)
            {
                return new MenuItemValidationArgs(m_item.Command) { IsValid = false, IsVisible = false };
            }

            if(m_item.Validate == null)
            {
                return new MenuItemValidationArgs(m_item.Command) { IsValid = true, IsVisible = true };
            }

            MenuItemValidationArgs args = new MenuItemValidationArgs(m_item.Command);
            m_item.Validate.Invoke(args);
            return args;
        }

    }
}

