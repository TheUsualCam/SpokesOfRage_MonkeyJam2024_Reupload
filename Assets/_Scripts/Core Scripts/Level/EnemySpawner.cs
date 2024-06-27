using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Level
{
    public class EnemySpawner : MonoBehaviour
    {
        public SectionManager SectionManager;

        [Space]
        public Transform[] SpawnPositions;
        public List<GameObject> EnemyPrefabs;
        public int EnemyCounter;
        public int EnemiesToSpawn;

        [Space] 
        public float SpawnInterval;

        private int _minAmount = 2;
        private int _maxAmount = 5;

        public bool IsSpawning { get; private set; }

        void Update()
        {
            if (SectionManager.InSection)
                StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            IsSpawning = true;

            RandomizeEnemiesToSpawn();
            while (EnemyCounter < EnemiesToSpawn)
            {
                // spawn enemy
                for (int i = 0; i < EnemiesToSpawn; i++)
                {
                    EnemyCounter++;
                    Instantiate(RandomEnemy(), new Vector3(RandomPos().position.x, RandomPos().position.y, 0), Quaternion.identity);
                    yield return new WaitForSeconds(SpawnInterval);
                }
            }

            IsSpawning = false;
        }

        void RandomizeEnemiesToSpawn()
        {
            EnemiesToSpawn = Random.Range(_minAmount, _maxAmount);
        }

        GameObject RandomEnemy()
        {
            int r = Random.Range(0, EnemyPrefabs.Count);
            return EnemyPrefabs[r];
        }

        Transform RandomPos()
        {
            int r = Random.Range(0, SpawnPositions.Length);
            return SpawnPositions[r];
        }
    }
}