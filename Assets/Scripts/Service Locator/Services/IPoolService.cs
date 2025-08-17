using UnityEngine;

namespace CatInTheAlley.ServiceLocator {
    public interface IPoolService {
        public GameObject SpawnFromPool(
            string itemName,
            Vector3 position,
            Quaternion rotation = default,
            Transform parent = null);

        public void ReturnToPool(
            string itemName,
            GameObject objectToReturn,
            Transform position = null,
            Quaternion rotation = default,
            Transform parent = null);
    }
}
