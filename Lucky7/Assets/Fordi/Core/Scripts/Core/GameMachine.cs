﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fordi.Common;
using Fordi.UI.MenuControl;
using Fordi.UI;

namespace Fordi.Core
{
    public enum Playground
    {
        HOME,
        LUCKY7
    }

    public enum GameplayMode
    { 
        NONE,
        PAUSED,
        RUNNING,
    }

    public interface IGame
    {
        bool CanExecuteMenuCommand(string cmd);
        void ExecuteMenuCommand(MenuClickArgs args);
        void Play();
        void Pause();
        void Resume();
        void Stop();
        void ToggleMenu();
        void GoBack();
        void OpenMenu();
        void OpenGridMenu(MenuCommandType commandType);
        void UpdateResourceSelection(MenuClickArgs args);
        void Load();
        void Unload();
    }

    public interface IGameMachine
    {
        void ExecuteMenuCommand(MenuClickArgs args);
        void CanExecuteMenuCommand(MenuItemValidationArgs args);
        IGame GetGame(Playground game);
        IGame GetGame(string name);
        void SetGame(IGame game);
        void UpdateResourceSelection(MenuClickArgs args);
        void LoadGame();
    }

    /// <summary>
    /// Uses state design pattern.
    /// All Game classes are its states.
    /// </summary>
    public class GameMachine : MonoBehaviour, IGameMachine
    {
        private IGame m_home, m_lucky7, m_currentGame;
        private IMenuSelection m_menuSelection;
        private IAudio m_audio;
        

        private void Awake()
        {
            m_home = GetComponentInChildren<Home>();
            m_lucky7 = GetComponentInChildren<Lucky7Engine.Lucky7>();
            m_menuSelection = IOC.Resolve<IMenuSelection>();
            m_audio = IOC.Resolve<IAudio>();
            SetGame(GetGame(m_menuSelection.Playground));
            m_menuSelection.Playground = Playground.LUCKY7;
            m_currentGame.Load();
        }

        public IGame GetGame(Playground game)
        {
            switch (game)
            {
                case Playground.HOME:
                    return m_home;
                case Playground.LUCKY7:
                    return m_lucky7;
                default:
                    return null;
            }
        }

        public void SetGame(IGame game)
        {
            m_currentGame = game;
        }

        #region GAME_INTERFACE
        public void CanExecuteMenuCommand(MenuItemValidationArgs args)
        {
            args.IsValid = m_currentGame.CanExecuteMenuCommand(args.Command);
        }

        public void ExecuteMenuCommand(MenuClickArgs args)
        {
            m_currentGame.ExecuteMenuCommand(args);
        }
        
        public void Play()
        {
            m_currentGame.Play();
        }

        public void Pause()
        {
            m_currentGame.Pause();
        }

        public void Resume()
        {
            m_currentGame.Resume();
        }

        public void Stop()
        {
            m_currentGame.Stop();
        }
        
        public void ToggleMenu()
        {
            m_currentGame.ToggleMenu();
        }

        public void GoBack()
        {
            m_currentGame.GoBack();
        }

        public IGame GetGame(string name)
        {
            switch (name.ToLower())
            {
                case "home":
                    return m_home;
                case "lucky7":
                    return m_lucky7;
                default:
                    return null;
            }
        }
        
        public void UpdateResourceSelection(MenuClickArgs args)
        {

        }

        public void LoadGame()
        {
            //AudioArgs args = new AudioArgs(m_menuSelection.Music);
            //args.FadeTime = 2;
            //args.Done = () => SceneManager.LoadScene(m_menuSelection.Location);
            //m_audio.Pause(args);
            //m_currentGame.OnLoad
            IOC.Resolve<IGlobalUI>().CloseMenu();
            m_currentGame.Unload();
            m_currentGame = GetGame(m_menuSelection.Playground);
            m_currentGame.Load();
        }
        #endregion
    }
}
