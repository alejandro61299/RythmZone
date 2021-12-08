using System.Collections;
using Core.Singleton;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Level
{
    public class LevelLoader : SingletonBehaviour<LevelLoader>
    {
        private AsyncOperationHandle<GameObject> _operation;
        private IEnumerator Start()
        {
            yield return _operation = Addressables.InstantiateAsync("TestLevel");
            LevelManager levelManager = _operation.Result.GetComponent<LevelManager>();
            levelManager.Initialize();
        }
    }
}