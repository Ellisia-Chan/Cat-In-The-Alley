using UnityEngine;

using CatInTheAlley.Interfaces;
using CatInTheAlley.EventSystem;
using CatInTheAlley.PlayerSystem.Events;

namespace CatInTheAlley.GrabSystem {
    public class GrabController : MonoBehaviour {
        [Header("Hand Point")]
        [SerializeField] private Transform handPoint;

        private IGrabbable heldItem;

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }

        private void OnInteractAction(EVT_OnPlayerInteractAction evt) {
            if (heldItem != null) {
                DropHeldItem();
            }
        }

        public void TryGrab(IGrabbable grabbable) {
            if (heldItem == null) {
                heldItem = grabbable;
                heldItem.OnGrab(handPoint);

            }
            else {
                DropHeldItem();
            }
        }

        public void DropHeldItem() {
            if (heldItem != null) {
                heldItem.OnDrop(handPoint.position + transform.forward * 0.5f);
                heldItem = null;
            }
        }
    }

}