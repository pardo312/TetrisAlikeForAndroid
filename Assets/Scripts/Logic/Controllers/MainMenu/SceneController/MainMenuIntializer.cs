using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{

    public class MainMenuIntializer : MonoBehaviour, IInitializable
    {
        public string m_sceneName => SceneNames.MAIN_MENU;

        public void GetData(Action<object> callback)
        {
            callback?.Invoke(null);
        }

        public void GetTestData(Action<object> callback)
        {
            callback?.Invoke(null);
        }
    }
}
