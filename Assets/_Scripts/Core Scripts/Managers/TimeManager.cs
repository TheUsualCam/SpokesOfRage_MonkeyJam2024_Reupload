using UnityEngine;

namespace Assets._Scripts.Core_Scripts.Managers
{
    public class TimeManager : MonoBehaviour
    {
        private float _timer = 0;
        private bool _isPlaying = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_isPlaying)
            {
                _timer += Time.deltaTime;
            }
        }

        public void StartTimer()
        {
            _isPlaying = true;
        }

        public void EndTimer()
        {
            _isPlaying = false;
        }

        public int GetTime()
        {
            return (int)_timer;
        }
    }
}
