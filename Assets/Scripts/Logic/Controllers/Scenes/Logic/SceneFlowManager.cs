using JiufenPackages.SceneFlowManager.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiufenPackages.SceneFlowManager.Logic
{
    public class SceneFlowManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private SceneFlowConfigScriptable sceneFlowConfigScriptable;
        private string m_previousScene = "";
        #endregion Fields

        #region Methods
        #region RuntimeInit
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInit()
        {
            foreach (GameObject commonObject in Resources.LoadAll("Common", typeof(GameObject)))
            {
                Instantiate(commonObject);
            }
            SceneManager.sceneLoaded += m_instance.InitScene;
        }
        #endregion RuntimeInit

        #region Singleton
        [HideInInspector] public static SceneFlowManager m_instance;

        private void Awake()
        {
            //Change This
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }
        #endregion Singleton

        public void ChangeSceneTo(string nameOfScene)
        {
            m_previousScene = SceneManager.GetActiveScene().name;
            ShowLoadingScene();
            LoadScene(nameOfScene);
        }

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
        private void LoadScene(string nameOfScene)
        {
            if (sceneFlowConfigScriptable.scenesNames.Contains(nameOfScene))
            {
                SceneManager.LoadSceneAsync(nameOfScene);
            }
            else
            {
                Debug.Log($"Scene {nameOfScene} doesn't exist. Check the SceneFlowScriptable.");
            }
        }

        private void InitScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != sceneFlowConfigScriptable.loadingScene)
            {
                SceneController sceneController = FindObjectOfType<SceneController>();
                sceneController.Init(null, (successLoadingScene) =>
                {
                    List<string> sceneLoadedNames = new List<string>();
                    for (int i = 0; i < SceneManager.sceneCount; i++)
                    {
                        sceneLoadedNames.Add(SceneManager.GetSceneAt(i).name);
                    }

                    if (sceneLoadedNames.Contains(sceneFlowConfigScriptable.loadingScene))
                    {
                        HideLoadingScene(successLoadingScene);
                    }
                });
            }
        }
        #endregion Methods
    }
}
