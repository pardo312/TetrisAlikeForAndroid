using JiufenPackages.SceneFlow.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenPackages.SceneFlow.Logic
{
    public class SceneFlowManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private SceneFlowConfigScriptable sceneFlowConfigScriptable;
        private Dictionary<string, IInitializable> initilizables = new Dictionary<string, IInitializable>();
        private string m_previousScene = "";

        #endregion Fields

        #region Methods

        #region Init

        public void Init()
        {
            for (int i = 0; i < GetComponents<IInitializable>().Length; i++)
            {
                this.initilizables.Add(GetComponents<IInitializable>()[i].m_sceneName, GetComponents<IInitializable>()[i]);
            }
        }

        #endregion Init

        #region Change Scene

        public void ChangeSceneTo(string nameOfScene)
        {
            m_previousScene = SceneManager.GetActiveScene().name;
            ShowLoadingScene();
            LoadScene(nameOfScene);
        }

        private void LoadScene(string nameOfScene)
        {
            if (sceneFlowConfigScriptable.scenesNames.Contains(nameOfScene))
            {
                SceneManager.LoadSceneAsync(nameOfScene);
            }
            else
            {
                if (nameOfScene.CompareTo(m_previousScene) != 0)
                {
                    SceneManager.LoadScene(m_previousScene);
                    Debug.Log($"Scene {nameOfScene} doesn't exist. Check the SceneFlowScriptable.");
                }
                else
                {
                    Debug.Log($"Previours Scene {m_previousScene} doesn't exist. Check the SceneFlowScriptable.");
                }
            }
        }

        #endregion Change Scene

        #region LoadingScene

        private void ShowLoadingScene()
        {
            SceneManager.LoadSceneAsync(sceneFlowConfigScriptable.loadingScene);
        }

        private void HideLoadingScene(bool loadingSuccess)
        {
            if (loadingSuccess)
            {
                SceneManager.UnloadSceneAsync(sceneFlowConfigScriptable.loadingScene);
            }
        }

        #endregion LoadingScene

        #region Init Scene

        public void InitScene(string sceneName)
        {
            if (sceneName != sceneFlowConfigScriptable.loadingScene)
            {
                initilizables[sceneName].GetData(InitializeSceneController);
            }
        }

        private void InitializeSceneController(object data)
        {
            SceneController sceneController = FindObjectOfType<SceneController>();
            sceneController.Init(data, (successLoadingScene) =>
            {
                if (CheckIfSceneExist())
                    HideLoadingScene(successLoadingScene);
            });
        }

        private bool CheckIfSceneExist()
        {
            List<string> sceneLoadedNames = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneLoadedNames.Add(SceneManager.GetSceneAt(i).name);
            }

            if (sceneLoadedNames.Contains(sceneFlowConfigScriptable.loadingScene))
            {
                return true;
            }
            return false;
        }

        #endregion Init Scene

        #endregion Methods
    }
}