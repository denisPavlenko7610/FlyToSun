using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Spawner : MonoBehaviour
    {
        public Transform[] spawnPoint;
        public SpawnData[] spawnData;
        public float levelTime;

        int level;
        float timer;

        void Awake()
        {
            spawnPoint = GetComponentsInChildren<Transform>();
            levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        }

        void Update()
        {
            if (!GameManager.instance.isLive)
                return;

            timer += Time.deltaTime;
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

            if (timer > spawnData[level].spawnTime) {
                timer = 0;
                Spawn();
            }
        }

        void Spawn()
        {
            GameObject enemy = GameManager.instance.pool.Get(0);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
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
