using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Parralax
{
    [ExecuteInEditMode]
    public class ParallaxCamera : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float deltaMovement);
        public ParallaxCameraDelegate OnCameraTranslate;

        private float _oldPosition;

        void Start()
        {
            _oldPosition = transform.position.x;
        }

        void Update()
        {
            if (transform.position.x != _oldPosition)
            {
                if (OnCameraTranslate != null)
                {
                    float delta = _oldPosition - transform.position.x;
                    OnCameraTranslate(delta);
                }

                _oldPosition = transform.position.x;
            }
        }
    }
}