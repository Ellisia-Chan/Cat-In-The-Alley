using UnityEngine;

using CatInTheAlley.EventSystem;
using CatInTheAlley.PlayerSystem.Events;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.SO;
using CatInTheAlley.Interfaces;

namespace CatInTheAlley.GrabSystem {
    public class GrabController : MonoBehaviour {
        [Header("Hand Point")]
        [SerializeField] private Transform handPoint;

        private IGrabbable grabbable;
        private GrabbableItemSO heldItem;

        private GameObject objectHeld;

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }

        private void OnInteractAction(EVT_OnPlayerInteractAction evt) {
            if (heldItem != null) {
                DropHeldItem(heldItem, grabbable);
            }
        }

        public void TryGrab(GrabbableItemSO grabbableSO, IGrabbable grabbable) {
            if (heldItem == null) {
                heldItem = grabbableSO;
                this.grabbable = grabbable;
                grabbable.OnGrab();
                objectHeld = PoolRuntimeSystem.Instance.SpawnFromPool(grabbableSO.nonRB_poolItem.name, handPoint.position, handPoint.rotation, handPoint);

            }
            else {
                DropHeldItem(heldItem, grabbable);
            }
        }

        public void DropHeldItem(GrabbableItemSO grabbableSO, IGrabbable grabbable) {
            if (heldItem != null) {
                PoolRuntimeSystem.Instance.ReturnToPool(grabbableSO.nonRB_poolItem.name, objectHeld);
                grabbable.OnDrop(handPoint.position);
                heldItem = null;
            }
        }
    }

}