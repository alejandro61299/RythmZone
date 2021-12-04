using System;
using System.Collections;
using System.Collections.Generic;
using Core.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneLoader
{
    public class SceneLoader : SingletonBehaviour<SceneLoader>
    {
        public enum SceneID { MainMenu , Game, Test}

        public static event Action OnStartLoad;
        public static event Action OnCompleteLoad;
        public static event Action<float> OnUpdateLoadProgression;
        
        private readonly WaitForSeconds _waitForProgressionEvent = new (0.2f);
        
        public static void LoadScene(SceneID sceneID)
        {
            if (!IsInstanced) return;
            Instance.StartCoroutine(Instance.StartLoad(sceneID));
        }
        
        private IEnumerator StartLoad(SceneID sceneID)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID.ToString());
            operation.completed += CompletedLoad;
            OnStartLoad?.Invoke();
            yield return OnLoadScene(operation);
        }

        private void CompletedLoad(AsyncOperation operation)
        {
            operation.completed -= CompletedLoad;
            OnCompleteLoad?.Invoke();
        }
        private IEnumerator OnLoadScene(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                OnUpdateLoadProgression?.Invoke(operation.progress);
                yield return _waitForProgressionEvent;
            }
        }
        
    }
}
