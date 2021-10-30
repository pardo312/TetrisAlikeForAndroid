using JiufenPackages.SceneFlowManager.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenPackages.SceneFlowManager.Logic
{


    public abstract class SceneController : MonoBehaviour
    {
        #region Fields
        public static SceneController Instance;

        public Action<string, object, Action<object>> SaveDataEvent;
        #endregion Fields

        #region Methods
        //TODO: implement data with data initializers
        public abstract void Init(object data, Action<bool> callback = null);

        protected virtual void Awake()
        {
            Instance = this;
        }
        #endregion Methods
    }
}
