using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] prefabs;

        private List<GameObject>[] pools;

        private void Awake()
        {
            InitializePools();
        }

        public int GetPoolLength() => pools.Length;

        private void InitializePools()
        {
            pools = new List<GameObject>[prefabs.Length];

            for (int index = 0; index < pools.Length; index++)
            {
                pools[index] = new List<GameObject>();
            }
        }
        
        public GameObject Get(int index)
        {
            if (index < 0 || index >= prefabs.Length)
            {
                Debug.LogError("Index out of bounds: " + index);
                return null; // Return null if index is invalid
            }

            foreach (GameObject item in pools[index])
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    return item;
                }
            }

            GameObject newObject = Instantiate(prefabs[index], transform);
            pools[index].Add(newObject);
            return newObject;
        }
    }
}