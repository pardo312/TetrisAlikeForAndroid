using JiufenPackages.SceneFlowManager.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        SceneFlowManager.m_instance.ChangeSceneTo(SceneNames.GAMEPLAY);

    }
}
