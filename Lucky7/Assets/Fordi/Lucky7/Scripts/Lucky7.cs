using Fordi.Common;
using Fordi.Core;
using Fordi.UI;
using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Lucky7Engine
{
    public class Lucky7 : Game
    {
        public const string Lucky7Help = "Rule 1: Minimum bid amount is Rs. 100.\n\n" +
                 "Rule 2: For Slot 1 to 6 and 8 to 12, bid amount doubles on win.\n\n" +
                 "Rule 3: For slot 7, bid amount tripples on win.";

        private List<Player> m_players = new List<Player>();

        private Lucky7Interface m_interface;

        private IEnumerator m_round;

        private int m_maximumBid = 0;

        public int Time { get; protected set; }

        private void StartSimulation()
        {
            m_state = GameState.ROUND_BEGAN;

            if (m_round != null)
                StopCoroutine(m_round);

            Time = Random.Range(5, 10);

            m_round = CoRound();
            StartCoroutine(m_round);

            Notify();
        }

        private IEnumerator CoRound()
        {
            while (Time > 0)
            {
                yield return new WaitForSeconds(1);
                if (Random.Range(0, 10) < 7)
                    CreateNewPlayer();
                if (Random.Range(0, 10) < 4)
                    CreateNewPlayer();
                if (Random.Range(0, 10) == 0)
                    CreateNewPlayer();
            }
        }

        void CreateNewPlayer()
        {
            var player = Player.CreateRandomPlayer();
            //Debug.LogError(player.LastBid);
            m_globalUI.SwapPlayer(player);
            m_players.Add(player);
        }

        public override void Load()
        {
            base.Load();
            m_globalUI.LoadHeader();
            
            m_player.Init(2500, m_globalUI.GetRandomAvatar(), 0, 0, 0);

            m_globalUI.Popup(new PopupInfo
            {
                Content = Lucky7Help,
                Title = "LUCKY 7",
                Preview = null,
                Blocked = false
            });

            m_interface = FindObjectOfType<Lucky7Interface>();

            StartSimulation();
        }

        public override void ExecuteMenuCommand(MenuClickArgs args)
        {
            if (args == null)
                return;
            base.ExecuteMenuCommand(args);
            switch (args.Command)
            {
                case "back":
                    m_globalUI.CloseLastScreen();
                    break;
                case "settings":
                    break;
                case "help":
                    m_globalUI.Popup(new PopupInfo
                    {
                        Content = Lucky7Help,
                        Title = "LUCKY 7",
                        Preview = null,
                        Blocked = true
                    });
                    break;
                default:
                    break;
            }
        }


        public override void ExecuteButtonCommand(UserInputArgs args)
        {
            if (args == null)
                return;

            base.ExecuteButtonCommand(args);
        }
    }
}
