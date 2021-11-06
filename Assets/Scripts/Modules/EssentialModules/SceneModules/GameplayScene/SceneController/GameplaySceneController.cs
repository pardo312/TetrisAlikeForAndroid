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

        public override void Init(object _gameplayDataObj, Action<bool> _callback = null)
        {
            if (_gameplayDataObj.GetType() == typeof(GameplayData))
            {
                GameplayData gameplayData = _gameplayDataObj as GameplayData;
                m_gameplayController.Init(gameplayData.HighScore, gameplayData.LevelDifficultySpeed);
            }
            else
            {
                Debug.Log("Data not sent correctly to gameplaySceneControlller");
                Debug.Break();
            }

            m_playerBehaviour = new PlayerBehaviour();
            m_playerBehaviour.Init(m_gameplayController);

            m_gameplayController.OnGoBackTomainMenu -= GoBackToMainMenu;
            m_gameplayController.OnGoBackTomainMenu += GoBackToMainMenu;
        }

        public void GoBackToMainMenu(int highScore)
        {
            DataManager.ReadEvent(DataKeys.SAVE_HIGHSCORE, highScore, (callbackData) =>
            {
                m_playerBehaviour.DesuscribreInputsEvents();
                GameManager.m_instance.ChangeSceneTo(SceneNames.MAIN_MENU);
            });
        }
    }

    public class GameplayData
    {
        public float LevelDifficultySpeed;
        public int HighScore;
    }
}
