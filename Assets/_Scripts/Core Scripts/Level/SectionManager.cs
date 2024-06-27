using MonkeyJam.Assets._Scripts.Core_Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Level
{
    public class SectionManager : MonoBehaviour
    {
        [Header("Selection Setup")]
        public LayerMask EnemyLayer;
        public Vector2 SectionSize;

        [Header("Section Setup")] 
        public Transform[] Sections;

        [Header("Camera Movement Variables")] 
        public float Duration;
        public float Elapsed;

        private List<GameObject> _enemiesInSection = new();
        private Camera _mainCam;
        private int _currentSectionIndex= 0;
        private bool _movingToNextSection = false;

        public bool InSection { get; private set; } = true;

        void Start()
        {
            _mainCam = Camera.main;

            // Immediately find the enemies in section
            FindEnemiesInSection();
        }

        void Update()
        {
            // Iterate through the update to find enemies
            FindEnemiesInSection();

            // if there are no more enemies, start coroutine
            if (_enemiesInSection.Count == 0 && !_movingToNextSection)
            {
                // Check if the player is at the final section
                if (_currentSectionIndex == Sections.Length - 1)
                {
                    if (!GameManager.Singleton.UiManager.GetIsGameWon())
                    {
                        GameManager.Singleton.UiManager.GameWin();
                        Debug.Log("YIPPEE");
                    }
                }
                else
                {
                    StartCoroutine(nameof(MoveCameraToNextSection));
                }
            }
        }

        private void FindEnemiesInSection()
        {
            // Clear the current list of Enemies
            _enemiesInSection.Clear();

            // Calculate the section center based on the camera's position
            Vector2 sectionCenter = _mainCam.transform.position;

            // Find all colliders within the section size
            Collider2D[] colliders = Physics2D.OverlapBoxAll(sectionCenter, SectionSize, 0, EnemyLayer);

            // iterate through the colliders and add them to the list
            foreach (var col in colliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    _enemiesInSection.Add(col.gameObject);
                }
            }
        }

        private IEnumerator MoveCameraToNextSection()
        {
            _movingToNextSection = true;
            InSection = false;

            if (_currentSectionIndex < Sections.Length - 1)
            {
                // Move to the next section
                _currentSectionIndex++;
                Vector3 nextSectionPosition = Sections[_currentSectionIndex].position;
                nextSectionPosition.z = -10f;

                // Smoothly move the camera to the next section
                Elapsed = 0f;
                Vector3 startingPosition = _mainCam.transform.position;

                while (Elapsed < Duration)
                {
                    _mainCam.transform.position =
                        Vector3.Lerp(startingPosition, nextSectionPosition, Elapsed / Duration);
                    Elapsed += Time.deltaTime;
                    yield return null;
                }

                _mainCam.transform.position = nextSectionPosition;
            }

            _movingToNextSection = false;
            InSection = true;
        }

        void OnDrawGizmos()
        {
            if (_mainCam == null)
                _mainCam = Camera.main;

            // draw the section boundary 
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_mainCam.transform.position, SectionSize);
        }
    }
}
