using System.Collections.Generic;
using UnityEngine;

namespace Tale
{
    public class ObjectPooler : MonoBehaviour
    {
        #region Inner Structures

        [System.Serializable]
        public class Pool
        {
            public GameObject prefab;
            public int initialSize;
            public Stack<GameObject> items;
        }

        #endregion

        #region Properties

        [SerializeField]
        private List<Pool> poolList;
        private Dictionary<string, Pool> pools;

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            ConstructPools();
        }

        #endregion

        #region Pooling Functions

        public string[] GetPoolsNames()
        {
            List<string> poolNames = new List<string>();
            Dictionary<string, Pool>.KeyCollection keys = pools.Keys;
            foreach (string key in keys)
            {
                poolNames.Add(key);
            }
            return poolNames.ToArray();
        }

        public GameObject Get(string objectType, Vector3 itemPosition, Quaternion itemRotation)
        {
            if (pools.ContainsKey(objectType))
            {
                GameObject objectToSpawn;
                if (pools[objectType].items.Count > 0)
                {
                    objectToSpawn = pools[objectType].items.Pop();
                }
                else
                {
                    objectToSpawn = CreatePooledObject(pools[objectType].prefab);
                }
                objectToSpawn.transform.parent = transform;
                objectToSpawn.transform.position = itemPosition;
                objectToSpawn.transform.rotation = itemRotation;
                objectToSpawn.SetActive(true);
                return objectToSpawn;
            }
            else
            {
                Debug.Log("Pool type " + objectType + " doesn't exist");
                return null;
            }
        }

        public void Recycle(PooledObject pooledObject)
        {
            string objectType = pooledObject.GetObjectType();
            if (pools.ContainsKey(objectType))
            {
                GameObject objectToSave = pooledObject.gameObject;
                objectToSave.SetActive(false);
                objectToSave.transform.parent = transform;
                objectToSave.transform.position = transform.position;
                objectToSave.transform.rotation = Quaternion.identity;
                pools[objectType].items.Push(objectToSave);
            }
            else
            {
                Debug.Log("Pool type " + objectType + " doesn't exist");
            }
        }

        public void RecycleAll()
        {
            PooledObject[] pooledObjects = GetComponentsInChildren<PooledObject>();
            foreach (PooledObject pooledObject in pooledObjects)
            {
                Recycle(pooledObject);
            }
        }

        #endregion

        #region Support Functions

        private void ConstructPools()
        {
            if (poolList.Count > 0)
            {
                pools = new Dictionary<string, Pool>();
                foreach (Pool pool in poolList)
                {
                    if (pool.initialSize > 0)
                    {
                        Stack<GameObject> poolItems = new Stack<GameObject>();
                        for (int i = 0; i < pool.initialSize; i++)
                        {
                            GameObject newPooledObject = CreatePooledObject(pool.prefab); 
                            poolItems.Push(newPooledObject);
                        }
                        pool.items = poolItems;                        
                        pools.Add(pool.prefab.name, pool);
                    }
                }
            }
        }

        private GameObject CreatePooledObject(GameObject poolPrefab)
        {
            
            PooledObject pooledObject = Instantiate(poolPrefab, transform.position, Quaternion.identity).GetComponent<PooledObject>();
            pooledObject.SetObjectPooler(this);
            pooledObject.SetObjectType(poolPrefab.name);
            pooledObject.gameObject.SetActive(false);
            pooledObject.gameObject.transform.parent = transform;  
            return pooledObject.gameObject;
        }

        #endregion
    }
}
