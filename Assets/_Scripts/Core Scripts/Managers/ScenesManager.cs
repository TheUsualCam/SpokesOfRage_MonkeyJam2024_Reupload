using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Managers
{
    public class ScenesManager : MonoBehaviour
    {
        public SceneTransition SceneTransition;

        private static ScenesManager _instance;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Method to load a scene asynchronously
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            if (SceneTransition != null)
            {
                Debug.Log("Starting coroutine");
                yield return StartCoroutine(
                    GameManager.Singleton.SceneTransitionManager.PlayTransitionIn(SceneTransition));
            }

            GameManager.Singleton.AudioManager.Stop(GetActiveSceneName());

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            while (!async.isDone)
                yield return null;

            GameManager.Singleton.AudioManager.Play(sceneName);

            if (SceneTransition != null)
            {
                yield return StartCoroutine(
                    GameManager.Singleton.SceneTransitionManager.PlayTransitionOut(SceneTransition));
            }
        }

        // Method to check if a scene is currently active
        public bool IsSceneActive(string sceneName)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            return activeScene.name == sceneName;
        }

        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
