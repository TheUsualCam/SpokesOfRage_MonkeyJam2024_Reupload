using MonkeyJam.Assets._Scripts.Core_Scripts.Managers;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Scene_Related
{
    public class EndLevelTrigger : MonoBehaviour
    {
        public string SceneToLoad;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameManager.Singleton.ScenesManager.LoadScene(SceneToLoad);
            }
        }
    }
}
