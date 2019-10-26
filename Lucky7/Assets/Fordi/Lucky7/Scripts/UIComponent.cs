using Fordi.Common;
using Fordi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.UI
{
    public class UIComponent : GameComponent
    {
        protected IGlobalUI m_globalUI;

        public override void AwakeOverride()
        {
            base.AwakeOverride();
            m_globalUI = IOC.Resolve<IGlobalUI>();
        }

        public virtual void OptionsClick() { }
        public virtual void SettingsClick() { }
        public virtual void HelpClick() { }
    }
}