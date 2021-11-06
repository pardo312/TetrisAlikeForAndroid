using JiufenPackages.GameManager.Logic;
using JiufenPackages.SceneFlow.Logic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public float[] difficultyArray = new float[3];
    public void LoadGameplayScene(int _levelDifficultySpeed)
    {
        float speed = 0;
        switch (_levelDifficultySpeed)
        {
            case 1:
                speed = difficultyArray[0];
                break;
            case 2:
                speed = difficultyArray[1];
                break;
            case 3:
                speed = difficultyArray[2];
                break;
        }

        DataManager.ReadEvent(DataKeys.SAVE_LEVEL_DIFFICULTY_SPEED, speed, (callbackData) =>
        {
            GameManager.m_instance.ChangeSceneTo(SceneNames.GAMEPLAY);
        });
    }
}