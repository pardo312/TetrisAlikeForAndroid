using JiufenGames.TetrisAlike.Logic;
using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

namespace JiufenGames.TetrisAlike.Logic
{
    public class GameplaySceneController : SceneController
    {
        public GameplayController m_gameplayController;

        public override void Init(object highScore, Action<bool> callback = null)
        {
            m_gameplayController.Init((int)highScore);
        }
    }
}
