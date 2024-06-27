using System.Collections;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using MonkeyJam._Scripts.Core_Scripts.PlayerRelated;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;

        public GameObject TitleScreen;
        public GameObject Settings;
        public GameObject PauseMenu;
        public GameObject VictoryMenu;
        public GameObject GameOverMenu;

        [Space]
        public GameObject LevelGUI;
        public Slider HealthBar;

        [SerializeField] private AudioSource _uiAudioSource;
        [SerializeField] private AudioClip _btnClickClip;

        [SerializeField] private bool _isPaused = false;
        [SerializeField] private bool _isGameOver = false;

        private bool _pressedButton;
        [SerializeField] private bool _isGameWon = false;

        private string _currentSceneName = "MainMenu";

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }

            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            GameManager.Singleton.ScenesManager.LoadScene("Level01");
            GameManager.Singleton.AudioManager.Stop(GameManager.Singleton.AudioManager.FirstSongToPlay);
            GameManager.Singleton.AudioManager.Play("Level01");
        }

        void Update()
        {
            if (GameManager.Singleton.ScenesManager.IsSceneActive("LoreScene"))
            {
                TitleScreen.SetActive(false);
                Settings.SetActive(false);
                PauseMenu.SetActive(false);
                VictoryMenu.SetActive(false);
                GameOverMenu.SetActive(false);
            }

            if (!GameManager.Singleton.ScenesManager.IsSceneActive("MainMenu"))
            {
                TitleScreen.SetActive(false);
                Settings.SetActive(false);
                LevelGUI.SetActive(true);


                // Get data from the player
                var playerData = FindObjectOfType<PlayerController>().gameObject.GetComponent<EntityData>();
                if (playerData != null)
                    HealthBar.value = playerData.CurrentHealth;

                if (GameManager.Singleton.InputManager.GetPauseInput() > 0 && !PauseMenu.activeInHierarchy)
                {
                    PauseGame();
                }

                if (playerData.CurrentHealth > 0)
                {
                    _isGameOver = false;
                }
            }
            else
            {
                _pressedButton = false;
                LevelGUI.SetActive(false);
                TitleScreen.SetActive(true);
                GameOverMenu.SetActive(false);
            }

            if ((!_isPaused && PauseMenu.activeInHierarchy) || GameManager.Singleton.ScenesManager.GetActiveSceneName() == "MainMenu")
            {
                _isPaused = false;
                PauseMenu.SetActive(false);
            }

            if ((!_isGameOver && GameOverMenu.activeInHierarchy) || GameManager.Singleton.ScenesManager.GetActiveSceneName() == "MainMenu")
            {
                _isGameOver = false;
                GameOverMenu.SetActive(false);
            }

            if(GameManager.Singleton.ScenesManager.GetActiveSceneName() != _currentSceneName)
            {
                _isGameWon = false;
                VictoryMenu.SetActive(false);
                _currentSceneName = GameManager.Singleton.ScenesManager.GetActiveSceneName();
            }

            if ((Time.timeScale < 1 && !_isPaused && !_isGameOver))
            {
                Time.timeScale = 1;
            }
            if(!_isGameOver)
            {
                GameManager.Singleton.AudioManager.Stop("Continue");
            }
            if(!_isGameWon)
            {
                GameManager.Singleton.AudioManager.Stop("Victory");
            }
        }

        public void PlayNextLevel()
        {
            switch(GameManager.Singleton.ScenesManager.GetActiveSceneName())
            {
                case "Level01":
                    GameManager.Singleton.AudioManager.Stop("Victory");
                    GameManager.Singleton.ScenesManager.LoadScene("Level02");
                    if (GameManager.Singleton.ScenesManager.IsSceneActive("Level02"))
                    {
                        GameManager.Singleton.AudioManager.Play("Level02");
                        _isGameWon = false;
                        VictoryMenu.SetActive(false);
                    }
                    break;
                case "Level02":
                    GameManager.Singleton.AudioManager.Stop("Victory");
                    GameManager.Singleton.ScenesManager.LoadScene("Level03");
                    if (GameManager.Singleton.ScenesManager.IsSceneActive("Level02"))
                    {
                        GameManager.Singleton.AudioManager.Play("Level03");
                        _isGameWon = false;
                        VictoryMenu.SetActive(false);
                    }
                    break;
            }
        }

        public void PlayMainMenu()
        {
            if (GameManager.Singleton.ScenesManager.IsSceneActive(GameManager.Singleton.ScenesManager.GetActiveSceneName()))
            {
                // stop the current scene's song
                string s = GameManager.Singleton.ScenesManager.GetActiveSceneName();
                GameManager.Singleton.AudioManager.Stop(s);
            }
        }

        public void RetryLevel()
        {
            _isPaused = false;
            _isGameOver = false;
            _pressedButton = false;
            GameManager.Singleton.AudioManager.Stop("Continue");
            GameManager.Singleton.AudioManager.Stop(GameManager.Singleton.ScenesManager.GetActiveSceneName());
            GameManager.Singleton.ScenesManager.LoadScene(GameManager.Singleton.ScenesManager.GetActiveSceneName());
        }

        public void ReturnToMenu()
        {
            _pressedButton = true;
            _isPaused = false;
            _isGameOver = false;
            GameManager.Singleton.AudioManager.Stop("Victory");
            GameManager.Singleton.AudioManager.Stop("Continue");
            GameManager.Singleton.AudioManager.Stop(GameManager.Singleton.ScenesManager.GetActiveSceneName());
            GameManager.Singleton.ScenesManager.LoadScene("MainMenu");
            PlayMainMenu();
        }

        public void PauseGame()
        {
            _isPaused = true;
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void GameOver()
        {
            if (!_isGameOver && !_pressedButton)
            {
                _isGameOver = true;
                GameManager.Singleton.AudioManager.Stop(GameManager.Singleton.ScenesManager.GetActiveSceneName());
                GameManager.Singleton.AudioManager.Play("Continue");
                StartCoroutine(WaitForGameOverScreen());
            }
        }

        public void GameWin()
        {
            _isGameWon = true;
            GameManager.Singleton.AudioManager.Stop(GameManager.Singleton.ScenesManager.GetActiveSceneName());
            GameManager.Singleton.AudioManager.Play("Victory");
            VictoryMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void ResetGameCondition()
        {
            _isGameWon = false;
            VictoryMenu.SetActive(false);
            GameManager.Singleton.AudioManager.Stop("Victory");
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            _isPaused = false;
        }

        public bool GetIsGameOver()
        {
            return _isGameOver;
        }

        public bool GetIsGameWon()
        {
            return _isGameWon;
        }

        public void PlayButtonClickSound()
        {
            _uiAudioSource.clip = _btnClickClip;
            _uiAudioSource.Play();
        }

        IEnumerator WaitForGameOverScreen()
        {
            yield return new WaitForSeconds(1.0f);
            GameOverMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
