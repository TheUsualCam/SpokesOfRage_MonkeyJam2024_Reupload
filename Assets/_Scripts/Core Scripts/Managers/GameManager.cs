using Assets._Scripts.Core_Scripts.Managers;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Singleton;

        public InputManager InputManager;
        public UIManager UiManager;
        public AudioManager AudioManager;
        public ScenesManager ScenesManager;
        public SceneTransitionManager SceneTransitionManager;

        private void Awake()
        {
            if(Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(this);
            }
            else if(Singleton != this)
            {
                Destroy(gameObject);
            }

            InputManager = FindAnyObjectByType<InputManager>();
            UiManager = FindAnyObjectByType<UIManager>();
            AudioManager = FindAnyObjectByType<AudioManager>();
            ScenesManager = FindAnyObjectByType<ScenesManager>();
            SceneTransitionManager = FindAnyObjectByType<SceneTransitionManager>();
        }
    }
}
