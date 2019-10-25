﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Common;
using Fordi.UI;
using Fordi.UI.MenuControl;

namespace Fordi.Core
{
    public class Home : Game
    {
        public override void ExecuteMenuCommand(MenuClickArgs args)
        {
            base.ExecuteMenuCommand(args);
        }

        protected override void ExecuteSelectionCommand(MenuClickArgs args)
        {
            base.ExecuteSelectionCommand(args);
        }

        public override void Pause()
        {
            base.Pause();
            Debug.LogError("In home");
        }

        public override void Play()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            base.Resume();
            Debug.LogError("In home");
        }

        public override void Stop()
        {
            base.Stop();
            Debug.LogError("Already in home");
        }

        public override void UpdateResourceSelection(MenuClickArgs args)
        {
            base.UpdateResourceSelection(args);
            if (args.Data != null && args.Data is GameSource)
            {
                GameSource resource = (GameSource)args.Data;
                if (Enum.TryParse(args.Name.ToUpper(), out Playground type))
                    m_menuSelection.Playground = type;
            }
        }

        private int m_sequenceIterator = 0;
        public override void ToggleMenu()
        {
            if (m_globalUI == null)
            {
                m_globalUI = IOC.Resolve<IGlobalUI>();
            }

            if (!m_globalUI.IsMenuOpen)
            {
                base.ToggleMenu();
                m_sequenceIterator = 0;
            }
        }

        public override void Load()
        {
            base.Load();
            if (m_experienceMachine == null)
                m_experienceMachine = IOC.Resolve<IGameMachine>();
        }
    }
}