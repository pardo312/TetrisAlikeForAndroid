using JiufenPackages.GameManager.Logic;
using JiufenGames.TetrisAlike.Logic;
using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class GameplaySceneController : SceneController
    {
        public GameplayController m_gameplayController;
        private PlayerBehaviour m_playerBehaviour;

        public override void Init(object highScore, Action<bool> callback = null)
        {
            m_gameplayController.Init((int)highScore);
            m_playerBehaviour = new PlayerBehaviour();
            m_playerBehaviour.Init(m_gameplayController);

            m_gameplayController.OnResetScene -= OnReset;
            m_gameplayController.OnResetScene += OnReset;
        }

        public void OnReset(int highScore)
        {
            DataManager.ReadEvent(DataKeys.SAVE_HIGHSCORE, highScore, null);
            m_playerBehaviour.DesuscribreInputsEvents();
            GameManager.m_instance.ChangeSceneTo(SceneNames.GAMEPLAY);
        }
    }
}
