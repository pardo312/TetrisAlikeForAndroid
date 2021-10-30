using JiufenPackages.SceneFlowManager.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneController : SceneController
{
    public override void Init(object data, Action<bool> callback = null)
    {
        callback?.Invoke(true);
    }

}
