using System.Collections;
using Core.SceneLoader;
using Game.UI;
using UnityEngine;

namespace Game.Menus
{
    public class LoadingScreen : MenuUI
    {
        private static readonly int SHOW_ID = Animator.StringToHash("isShown");

        [SerializeField] private Animator _animator;
        [SerializeField] private TextUI _progress;
        private Coroutine _waitKey;

        private void Awake()
        {
            SceneLoader.OnStartLoad += ShowLoadScreen;
            SceneLoader.OnCompleteLoad += HideLoadScreen;
            SceneLoader.OnUpdateProgression += UpdateProgress;
        }
        private void OnDestroy()
        {
            SceneLoader.OnStartLoad -= ShowLoadScreen;
            SceneLoader.OnCompleteLoad -= HideLoadScreen;
            SceneLoader.OnUpdateProgression -= UpdateProgress;
        }
        private void ShowLoadScreen()
        {
            _animator.SetBool(SHOW_ID, true);
        }
        private void HideLoadScreen()
        {
            _animator.SetBool(SHOW_ID, false);
        }
        private void UpdateProgress(float progress)
        {
            _progress.SetText(progress >= 0.9f ? "Press B to continue..." : progress + " %" );
            _waitKey ??= StartCoroutine(WaitAnyKeyDown());
        }

        private IEnumerator WaitAnyKeyDown()
        {
            yield return null;
            while (!Input.anyKeyDown) yield return null;
            SceneLoader.ActiveScene();
        }
    }
}