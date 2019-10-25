using Fordi.Common;
using Fordi.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public class InputBase : MonoBehaviour
    {
        protected IGameMachine m_gameMachine;
        protected IGlobalUI m_globalUI;
        protected IPlayer m_player;

        protected void Awake()
        {
            AwakeOverride();
            m_gameMachine = IOC.Resolve<IGameMachine>();
            m_globalUI = IOC.Resolve<IGlobalUI>();
            m_player = IOC.Resolve<IPlayer>();
        }

        protected void OnDestroy()
        {
            OnDestroyOverride();
        }

        public virtual void AwakeOverride() { }
        public virtual void OnDestroyOverride() { }
        public virtual void OptionsClick() { }
        public virtual void SettingsClick() { }
        public virtual void HelpClick() { }
    }
}
