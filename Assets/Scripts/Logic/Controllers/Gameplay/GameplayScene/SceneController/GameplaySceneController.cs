using JiufenPackages.SceneFlow.Logic;
using System;

namespace JiufenGames.TetrisAlike.Logic
{
    public class GameplaySceneController : SceneController
    {
        public GameplayController m_gameplayController;
        private PlayerBehaviour m_playerBehaviour = new PlayerBehaviour();

        public override void Init(object highScore, Action<bool> callback = null)
        {
            m_gameplayController.Init((int)highScore);
            m_playerBehaviour.Init(m_gameplayController);
        }
    }
}