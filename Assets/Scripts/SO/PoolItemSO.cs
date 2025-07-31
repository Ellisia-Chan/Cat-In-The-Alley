using UnityEngine;

namespace CatInTheAlley.SO {

	[CreateAssetMenu(fileName = "PoolItem", menuName = "ScriptableObjects/PoolItem")]
	public class PoolItemSO : ScriptableObject {

		[HideInInspector] public string itemName;
        public GameObject prefab;
        public int poolSize;
        public Vector3 resetPosition = new Vector3(0, -100f, 0);

        private void OnValidate() {
            if (string.IsNullOrEmpty(itemName)) {
                itemName = name;
            }
        }
    }
}