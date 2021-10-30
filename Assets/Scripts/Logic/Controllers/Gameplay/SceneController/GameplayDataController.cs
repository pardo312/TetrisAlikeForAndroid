using System;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{
    public static class GameplayDataController
    {
        #region ----Methods---- 
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RuntimeInit()
        {
            DataManager.AddListeners(DataKeys.SAVE_HIGHSCORE, SaveHighScore);
            DataManager.AddListeners(DataKeys.LOAD_HIGHSCORE, LoadHighScore);
        }

        #region Save Data
        private static void SaveHighScore(object dataToSave, Action<object> callback = null)
        {
            if (dataToSave.GetType() == typeof(int))
            {
                PlayerPrefs.SetInt("HighScore", (int)dataToSave);
                callback?.Invoke(new DataResponseModel(true, "Ok", 500));
            }
            else
            {
                callback?.Invoke(new DataResponseModel(false, "Type of data incorrect", 500));
            }
        }
        #endregion Save Data

        #region Load Data
        private static void LoadHighScore(object payload, Action<object> response)
        {
            response?.Invoke(PlayerPrefs.GetInt("HighScore"));
        }
        #endregion Load Data
        #endregion ----Methods----
    }
}
