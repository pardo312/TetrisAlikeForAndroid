using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.SceneFlowManager.Model
{
    [CreateAssetMenu(fileName = "SceneFlowConfig.asset", menuName = "SceneFlow/SceneFlowConfig")]
    public class SceneFlowConfigScriptable : ScriptableObject
    {
        public string loadingScene;
        public List<string> scenesNames;
    }
}
