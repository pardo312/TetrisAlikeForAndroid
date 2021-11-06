using JiufenGames.TetrisAlike.Logic;
using System;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{
    public class GameplayInitilizer : MonoBehaviour, IInitializable
    {
        public string m_sceneName => "Gameplay";

        public void GetData(Action<object> callback)
        {
            DataManager.ReadEvent(DataKeys.LOAD_HIGHSCORE, null, (highScore) =>
              {
                  DataManager.ReadEvent(DataKeys.LOAD_LEVEL_DIFFICULTY_SPEED, null, (levelOfDifficultySpeed) =>
                    {
                        callback?.Invoke(new GameplayData() { LevelDifficultySpeed = (float)levelOfDifficultySpeed, HighScore = (int)highScore });
                    });
              });
        }

        public void GetTestData(Action<object> callback)
        {
            callback?.Invoke(new GameplayData() { LevelDifficultySpeed = 0.01f, HighScore = 0 });
        }
    }
}