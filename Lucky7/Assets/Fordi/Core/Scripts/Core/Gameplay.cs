using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fordi.Common;
using Fordi.UI.MenuControl;
using System;

namespace Fordi.Core
{
    public abstract class Gameplay : Game
    {
        [SerializeField]
        protected AudioResource[] m_voiceOvers;

        [SerializeField]
        private MenuItemInfo[] m_insceneMenuItems = new MenuItemInfo[] { };

        public override void ExecuteMenuCommand(MenuClickArgs args)
        {
            base.ExecuteMenuCommand(args);
            if (args.CommandType == MenuCommandType.VO)
                OpenResourceWindow(m_voiceOvers, "SELECT VOICEOVER");
        }

        /// <summary>
        /// Gameplay resource selection happens here.
        /// </summary>
        /// <param name="args"></param>
        public override void UpdateResourceSelection(MenuClickArgs args)
        {
            base.UpdateResourceSelection(args);
            if (args.Data != null && args.Data is GameSource)
            {
                GameSource resource = (GameSource)args.Data;
               
                if (resource.ResourceType == ResourceType.AUDIO)
                    m_menuSelection.VoiceOver = Array.Find(m_voiceOvers, item => item.Clip == ((AudioResource)resource).Clip).Clip;
            }
            ToggleMenu();

            //Apply selection to gameplay.
            //Selection can be accessed from m_menuSelection object.

        }

        public override void ToggleMenu()
        {
            if (m_globalUI.IsOpen)
                m_menu.Close();
            else
                m_menu.Open(m_insceneMenuItems);
        }

        public override void Load()
        {
            base.Load();
            AudioArgs args = new AudioArgs(m_menuSelection.Music);
            args.FadeTime = 2;
            m_audio.Play(args);
        }
    }
}
