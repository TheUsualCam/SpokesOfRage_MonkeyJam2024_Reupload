using System.Collections;
using MonkeyJam.Assets._Scripts.Core_Scripts.Managers;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts
{
    public class LoreScene : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(LoadNextScene());
        }

        IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds(5f);
            GameManager.Singleton.ScenesManager.LoadScene("MainMenu");
        }
    }
}
