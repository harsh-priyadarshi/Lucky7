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

        protected void Awake()
        {
            AwakeOverride();
            m_gameMachine = IOC.Resolve<IGameMachine>();
            m_globalUI = IOC.Resolve<IGlobalUI>();
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
