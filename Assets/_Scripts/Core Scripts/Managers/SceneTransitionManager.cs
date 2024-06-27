using System.Collections;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Managers
{
    [System.Serializable]
    public class SceneTransition
    {
        public Animator TransitionAnimator;
        public AnimationClip TransitionOut;
        public AnimationClip TransitionIn;
        public float TransitionOutDuration;
        public float TransitionInDuration;
    }

    public class SceneTransitionManager : MonoBehaviour
    {
        private static SceneTransitionManager _instance;

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

        public IEnumerator PlayTransitionOut(SceneTransition transition)
        {
            if (transition.TransitionAnimator != null)
            {
                transition.TransitionAnimator.Play(transition.TransitionOut.name);
                yield return new WaitForSeconds(transition.TransitionOutDuration);
            }
        }

        public IEnumerator PlayTransitionIn(SceneTransition transition)
        {
            if (transition.TransitionAnimator != null)
            {
                transition.TransitionAnimator.Play(transition.TransitionIn.name);
                yield return new WaitForSeconds(transition.TransitionInDuration);
            }
        }
    }
}