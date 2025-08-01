using System.Collections.Generic;
using UnityEngine;

using CatInTheAlley.SO;
using CatInTheAlley.ObjectPoolSystem;

namespace CatInTheAlley.WorldSpawner {
    public class ObjectSpawnerManager : MonoBehaviour {
        [SerializeField] private List<ObjectSpawnPointSO> objectSpawnPointSOs;

        private void Start() {
            foreach (ObjectSpawnPointSO objectSpawnPointSO in objectSpawnPointSOs) {
                if (PoolRuntimeSystem.Instance != null) {
                    string itemName = objectSpawnPointSO.poolItem.itemName;

                    foreach (Vector3 objectSpawnPoint in objectSpawnPointSO.spawnPoints) {
                        PoolRuntimeSystem.Instance.SpawnFromPool(itemName, objectSpawnPoint, Quaternion.identity);
                    }
                }
            }
        }
    }
}