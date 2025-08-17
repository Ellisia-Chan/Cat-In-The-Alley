using System.Collections.Generic;
using UnityEngine;

using CatInTheAlley.SO;
using CatInTheAlley.ServiceLocator;

namespace CatInTheAlley.ObjectPoolSystem {
    public class PoolRuntimeSystem : MonoBehaviour, IPoolService {

        [Header("Pools")]
        [SerializeField] private List<PoolItemSO> pools;
        private Dictionary<string, Queue<GameObject>> poolDictionary;
        private Dictionary<string, PoolItemSO> poolItemDictionary;
        private Dictionary<string, Transform> poolParents;

        private void Awake() {

            if (ServiceRegistry.IsRegistered<IPoolService>()) {
                Debug.LogWarning("PoolRuntimeSystem: Service is already registered.");
                Destroy(gameObject);
                return;
            }

            ServiceRegistry.Register<IPoolService>(this);

            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            poolItemDictionary = new Dictionary<string, PoolItemSO>();
            poolParents = new Dictionary<string, Transform>();

            foreach (PoolItemSO pool in pools) {
                string parentName = "Pool_" + pool.itemName;
                GameObject parentObject = new GameObject(parentName);
                parentObject.transform.SetParent(transform);

                poolParents[pool.itemName] = parentObject.transform;

                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.poolSize; i++) {
                    GameObject obj = Instantiate(pool.prefab, pool.resetPosition, Quaternion.identity, parentObject.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.itemName, objectPool);
                poolItemDictionary.Add(pool.itemName, pool);
            }
        }



        public GameObject SpawnFromPool(string itemName, Vector3 position, Quaternion rotation = default, Transform parent = null) {
            if (!poolDictionary.ContainsKey(itemName)) {
                Debug.LogWarning("Pool with name " + itemName + " doesn't exist.");
                return null;
            }

            Queue<GameObject> poolQueue = poolDictionary[itemName];
            GameObject objectToSpawn = poolQueue.Dequeue();
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);

            if (parent != null) {
                objectToSpawn.transform.SetParent(parent);
            }

            return objectToSpawn;
        }

        public void ReturnToPool(string itemName, GameObject objectToReturn, Transform position = null, Quaternion rotation = default, Transform parent = null) {
            if (!poolDictionary.ContainsKey(itemName)) {
                Debug.LogWarning("Pool with name " + itemName + " doesn't exist.");
                return;
            }

            objectToReturn.SetActive(false);
            objectToReturn.transform.position = poolItemDictionary[itemName].resetPosition;
            objectToReturn.transform.rotation = Quaternion.identity;

            if (parent != null) {
                objectToReturn.transform.SetParent(parent);
            }
            else if (poolParents.TryGetValue(itemName, out Transform cachedParent)) {
                objectToReturn.transform.SetParent(cachedParent);
            }
            else {
                Debug.LogWarning($"No parent found for pool '{itemName}'");
            }

            poolDictionary[itemName].Enqueue(objectToReturn);
        }
    }
}
