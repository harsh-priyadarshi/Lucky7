using UnityEngine;
using UnityEngine.SceneManagement;
using Fordi.Common;
using Fordi.UI.MenuControl;
using Fordi.UI;

namespace Fordi.Core
{
    [DefaultExecutionOrder(-100)]
    public class GameDeps : MonoBehaviour 
    {
        private IGameMachine m_gameMachine;

        protected virtual IGameMachine GameMachine
        {
            get
            {
                GameMachine gameMachine = FindObjectOfType<GameMachine>();
                if(gameMachine == null)
                {
                    gameMachine = gameObject.AddComponent<GameMachine>();
                }
                return gameMachine;
            }
        }

        private IAppTheme m_appTheme;

        protected virtual IAppTheme AppTheme
        {
            get
            {
                AppTheme appTheme = FindObjectOfType<AppTheme>();
                if (appTheme == null)
                {
                    appTheme = gameObject.AddComponent<AppTheme>();
                }
                return appTheme;
            }
        }

        private IAudio m_audio;

        protected virtual IAudio Audio
        {
            get
            {
                Audio audio = FindObjectOfType<Audio>();
                if (audio == null)
                {
                    var obj = new GameObject("Audio");
                    audio = obj.AddComponent<Audio>();
                    audio.transform.parent = transform;
                    audio.transform.localPosition = Vector3.zero;
                }
                return audio;
            }
        }

        private IGlobalUI m_vRMenu;

        protected virtual IGlobalUI UI
        {
            get
            {
                GlobalUI vrMenu = FindObjectOfType<GlobalUI>();
                return vrMenu;
            }
        }

        private void Awake()
        {
            if(m_instance != null)
            {
                Debug.LogWarning("AnotherInstance of GameDeps exists");
            }
            m_instance = this;

            AwakeOverride();
        }

        protected virtual void AwakeOverride()
        {
            m_gameMachine = GameMachine;
            m_appTheme = AppTheme;
            m_audio = Audio;
            m_vRMenu = UI;
        }

        private void OnDestroy()
        {
            if(m_instance == this)
            {
                m_instance = null;
            }

            OnDestroyOverride();

            m_gameMachine = null;
        }

        protected virtual void OnDestroyOverride()
        {

        }


        private static GameDeps m_instance;
        private static GameDeps Instance
        {
            get
            {
                if(m_instance == null)
                {
                    GameDeps deps = FindObjectOfType<GameDeps>();
                    if (deps == null)
                    {
                        GameObject go = new GameObject("GameDeps");
                        go.AddComponent<GameDeps>();
                        go.transform.SetSiblingIndex(0);
                    }   
                    else
                    {
                        m_instance = deps;
                    }
                }
                return m_instance;
            }    
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if(!Application.isPlaying)
            {
                return;
            }

            RegisterExpDeps();
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void RegisterExpDeps()
        {
            IOC.RegisterFallback(() => Instance.m_gameMachine);
            IOC.RegisterFallback(() => Instance.m_appTheme);
            IOC.RegisterFallback(() => Instance.m_vRMenu);
            IOC.RegisterFallback(() => Instance.m_audio);
            if (IOC.Resolve<IMenuSelection>() == null)
                IOC.Register<IMenuSelection>(new MenuSelection());
        }

        private static void OnSceneUnloaded(Scene arg0)
        {
            m_instance = null;
        }
    }
}

