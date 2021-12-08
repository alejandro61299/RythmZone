using System;
using System.Collections;
using Core.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.ObjectsExtension;

namespace Core.SceneLoader
{
    public class SceneLoader : SingletonBehaviour<SceneLoader>
    {
        public enum SceneID { MainMenu , SampleScene, Test}
        public static event Action OnStartLoad;
        public static event Action OnCompleteLoad;
        public static event Action<float> OnUpdateProgression;

        private AsyncOperation _operation;
        
        public static void LoadScene(SceneID sceneID)
        {
            if (!IsInstanced || !Instance._operation.Null()) return;
            Instance.StartCoroutine(Instance.StartLoad(sceneID));
        }

        public static void ActiveScene()
        {
            if (!IsInstanced || Instance._operation.Null()) return;
            Instance._operation.allowSceneActivation = true;
        }
        
        private IEnumerator StartLoad(SceneID sceneID)
        {
            OnStartLoad?.Invoke();
            _operation = SceneManager.LoadSceneAsync(sceneID.ToString());
            _operation.allowSceneActivation = false;
            _operation.completed += CompletedLoad;
            yield return OnLoadScene();
        }

        private void CompletedLoad(AsyncOperation operation)
        {
            operation.completed -= CompletedLoad;
            _operation = null;
            OnCompleteLoad?.Invoke();
        }
        private IEnumerator OnLoadScene()
        {
            while (!_operation.isDone)
            {
                OnUpdateProgression?.Invoke(_operation.progress);
                yield return null;
            }
        }
    }
}
