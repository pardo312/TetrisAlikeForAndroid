using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        GameManager.m_instance.ChangeSceneTo(SceneNames.GAMEPLAY);
    }
}
