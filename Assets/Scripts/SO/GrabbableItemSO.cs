using UnityEngine;

namespace CatInTheAlley.SO {

    [CreateAssetMenu(menuName = "ScriptableObjects/GrabbableItemSO")]
    public class GrabbableItemSO : ScriptableObject {
        public string itemName;
        public GameObject prefab;
        public AudioClip grabSFX;
        public AudioClip dropSFX;

        public PoolItemSO nonRB_poolItem;
        public PoolItemSO RB_poolItem;
    }
}