using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;  // Changed from public to serialized for better encapsulation.
        [SerializeField] private SpawnData[] spawnData;  // Changed from public to serialized for better encapsulation.
        private float levelTime;

        private int currentLevel;
        private float spawnTimer;

        private const int MinimumSpawnPointIndex = 1; // Prevents the first spawn point from being selected.
        
        private void Awake()
        {
            InitializeSpawnPoints();
            CalculateLevelTime();
        }

        private void InitializeSpawnPoints()
        {
            spawnPoints = GetComponentsInChildren<Transform>();
        }

        private void CalculateLevelTime()
        {
            levelTime = GameManager.Instance.MaxGameTime / spawnData.Length;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsLive)
                return;

            UpdateSpawnTimer();
            CheckForSpawn();
        }

        private void UpdateSpawnTimer()
        {
            spawnTimer += Time.deltaTime;
            currentLevel = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / levelTime), spawnData.Length - 1);
        }

        private void CheckForSpawn()
        {
            if (spawnTimer > spawnData[currentLevel].spawnTime)
            {
                spawnTimer = 0; // Reset timer after spawning
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            GameObject enemy = GameManager.Instance.Pool.Get(0);
            enemy.transform.position = GetRandomSpawnPoint();
            enemy.GetComponent<Enemy>().Init(spawnData[currentLevel]);
        }

        private Vector3 GetRandomSpawnPoint()
        {
            int randomIndex = Random.Range(MinimumSpawnPointIndex, spawnPoints.Length);
            return spawnPoints[randomIndex].position;
        }
    }

    [System.Serializable]
    public class SpawnData
    {
        public float spawnTime;
        public int spriteType;
        public int health;
        public float speed;
    }
}
