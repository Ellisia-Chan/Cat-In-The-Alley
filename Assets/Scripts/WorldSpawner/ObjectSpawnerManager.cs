using System.Collections.Generic;
using UnityEngine;

using CatInTheAlley.SO;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.ServiceLocator;

namespace CatInTheAlley.WorldSpawner {
    public class ObjectSpawnerManager : MonoBehaviour {
        [SerializeField] private List<ObjectSpawnPointSO> objectSpawnPointSOs;

        private IPoolService poolService;

        private void Start() {
            poolService = ServiceRegistry.Get<IPoolService>();


            foreach (ObjectSpawnPointSO objectSpawnPointSO in objectSpawnPointSOs) {
                if (poolService == null) {
                    Debug.LogWarning("ObjectSpawnerManager: PoolService is null");
                    return;
                }

                string itemName = objectSpawnPointSO.poolItem.itemName;
                foreach (Vector3 objectSpawnPoint in objectSpawnPointSO.spawnPoints) {
                    poolService.SpawnFromPool(itemName, objectSpawnPoint, Quaternion.identity);
                }
            }
        }
    }
}