using UnityEngine;

namespace CatInTheAlley.SO {

    [CreateAssetMenu(menuName = "Scriptable Objects/GrabbableItemSO")]
    public class GrabbableItemSO : ScriptableObject {
        public string itemName;
        public GameObject prefab;
        public AudioClip grabSFX;
        public AudioClip dropSFX;
    }
}