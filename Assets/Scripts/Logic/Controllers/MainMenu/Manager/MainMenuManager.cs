using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        GameManager.m_instance.ChangeSceneTo(SceneNames.GAMEPLAY);
    }
}