using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Model
{
    [CreateAssetMenu(fileName = "SceneFlowConfig.asset", menuName = "SceneFlow/SceneFlowConfig")]
    public class SceneFlowConfigScriptable : ScriptableObject
    {
        public string loadingScene;
        public List<string> scenesNames;
    }
}