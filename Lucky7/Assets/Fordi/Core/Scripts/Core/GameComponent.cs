using Fordi.Common;
using Fordi.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Core
{
    public class GameComponent : MonoBehaviour, IObserver
    {
        protected IGameMachine m_gameMachine;
        protected IPlayer m_player;

        protected void Awake()
        {
            m_gameMachine = IOC.Resolve<IGameMachine>();
            m_player = IOC.Resolve<IPlayer>();
            AwakeOverride();
        }

        protected void OnDestroy()
        {
            OnDestroyOverride();
        }

        public virtual void AwakeOverride() { }
        public virtual void OnDestroyOverride() { }

        public virtual void GameUpdate() { }
    }
}
