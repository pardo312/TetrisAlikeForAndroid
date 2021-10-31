using System;

namespace JiufenPackages.SceneFlow.Logic
{
    public interface IInitializable
    {
        string m_sceneName { get; }

        void GetData(Action<object> callback);

        void GetTestData(Action<object> callback);
    }
}