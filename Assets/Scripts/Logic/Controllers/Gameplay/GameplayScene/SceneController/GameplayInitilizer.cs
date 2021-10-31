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
                  callback?.Invoke(highScore);
              });
        }

        public void GetTestData(Action<object> callback)
        {
            DataManager.ReadEvent(DataKeys.LOAD_HIGHSCORE, null, (highScore) =>
              {
                  callback?.Invoke(highScore);
              });
        }
    }
}