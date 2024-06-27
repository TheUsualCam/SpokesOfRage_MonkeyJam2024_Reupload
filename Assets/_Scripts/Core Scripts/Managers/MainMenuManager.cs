using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Scripts.Core_Scripts.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
