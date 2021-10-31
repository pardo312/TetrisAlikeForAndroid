using JiufenPackages.SceneFlow.Logic;
using System;
using UnityEngine;

public class MainMenuSceneController : SceneController
{
    [SerializeField] private MainMenuManager m_mainMenuManager;

    public override void Init(object _data, Action<bool> _callback = null)
    {
        _callback?.Invoke(true);
    }
}