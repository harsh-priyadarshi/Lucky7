using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Common;
using Fordi.UI;
using Fordi.UI.MenuControl;

namespace Fordi.Core
{
    public enum ResourceType
    {
        MUSIC,
        COLOR,
        MANDALA,
        LOCATION,
        AUDIO,
        EXPERIENCE
    }

    [System.Serializable]
    public class GameSource
    {
        public string Name;
        public Sprite Preview;
        public ResourceType ResourceType;
    }

    [System.Serializable]
    public class AudioResource : GameSource
    {
        public AudioClip Clip;
    }

    [System.Serializable]
    public class MandalaResource : GameSource
    {
        public GameObject Mandala;
    }

    [RequireComponent(typeof(Menu))]
    public abstract class Game : MonoBehaviour, IGame
    {
        protected IGameMachine m_experienceMachine;
        protected IGlobalUI m_globalUI;
        public IMenuSelection m_menuSelection;
        public IAudio m_audio;

        [SerializeField]
        private GameObject m_gamePrefab;

        [SerializeField]
        protected Menu m_menu;

        private GameObject m_gameInstance;

        protected void Awake()
        {
            m_experienceMachine = IOC.Resolve<IGameMachine>();
            m_menu = GetComponent<Menu>();
            m_globalUI = IOC.Resolve<IGlobalUI>();
            m_menuSelection = IOC.Resolve<IMenuSelection>();
            m_audio = IOC.Resolve<IAudio>();
            AwakeOverride();
        }

        protected virtual void AwakeOverride()
        {
            Init();
        }

        protected virtual void Init() { }

        protected MenuItemInfo[] ResourceToMenuItems(GameSource[] resources)
        {
            MenuItemInfo[] menuItems = new MenuItemInfo[resources.Length];
            for (int i = 0; i < resources.Length; i++)
            {
                menuItems[i] = new MenuItemInfo
                {
                    Path = resources[i].Name,
                    Text = resources[i].Name,
                    Command = resources[i].Name,
                    Icon = resources[i].Preview,
                    Data = resources[i],
                    CommandType = MenuCommandType.SELECTION
                };
            }
            return menuItems;
        }

        protected void OpenResourceWindow(GameSource[] resources, string windowTitle)
        {
            MenuItemInfo[] menuItems = ResourceToMenuItems(resources);
            m_menu.OpenGridMenu(menuItems, windowTitle);
        }

        /// <summary>
        /// Preserve selection into MenuSelection singleton m_menuSelection
        /// </summary>
        /// <param name="args"></param>
        protected virtual void ExecuteSelectionCommand(MenuClickArgs args)
        {
            UpdateResourceSelection(args);
        }

        public virtual bool CanExecuteMenuCommand(string cmd)
        {
            return true;
        }

        public virtual void ExecuteMenuCommand(MenuClickArgs args)
        {
            Debug.Log(args.Name + " " + args.Path + " " + args.Command + " " + args.CommandType.ToString());
        }

        public virtual void Pause() { }
       
        public virtual void Play() { }
        
        public virtual void Resume() { }
        
        public virtual void Stop() { }

        public virtual void ToggleMenu()
        {
            if (m_globalUI.IsMenuOpen)
                m_menu.Close();
            else
                m_menu.Open();
            
        }

        public void GoBack()
        {
            m_menu.Close();
            m_menu.Open();
        }

        public virtual void OpenMenu()
        {
            m_menu.Open();
        }

        public void OpenGridMenu(MenuCommandType commandType)
        {

        }

        public virtual void UpdateResourceSelection(MenuClickArgs args)
        {
            
        }

        public virtual void Load()
        {
            if (m_menuSelection == null)
                m_menuSelection = IOC.Resolve<IMenuSelection>();
            if (m_audio == null)
                m_audio = IOC.Resolve<IAudio>();
            m_gameInstance = Instantiate(m_gamePrefab);
        }

        public virtual void Unload()
        {
            if (m_menuSelection == null)
                m_menuSelection = IOC.Resolve<IMenuSelection>();
            if (m_audio == null)
                m_audio = IOC.Resolve<IAudio>();
            if (m_gameInstance != null)
                Destroy(m_gameInstance);
        }
    }
}
