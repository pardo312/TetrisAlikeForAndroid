using JiufenPackages.SceneFlow.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenPackages.SceneFlow.Logic
{


    public abstract class SceneController : MonoBehaviour
    {
        #region Singleton
        public static SceneController Instance;
        protected virtual void Awake()
        {
            Instance = this;
        }
        #endregion Singleton

        #region Methods
        public abstract void Init(object _data, Action<bool> _callback = null);
        #endregion Methods
    }
}
