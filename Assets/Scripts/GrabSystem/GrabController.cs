using CatInTheAlley.EventSystem;
using CatInTheAlley.Interfaces;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.PlayerSystem;
using CatInTheAlley.PlayerSystem.Events;
using CatInTheAlley.SO;

using UnityEngine;

namespace CatInTheAlley.GrabSystem {
    public class GrabController : MonoBehaviour {
        [Header("Hand Point")]
        [SerializeField] private Transform handPoint;
        [SerializeField] private Transform dropPoint;

        private Interactor interactor;

        private IGrabbable grabbable;
        private GrabbableItemSO heldItem;

        private GameObject objectHeld;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Awake() {
            interactor = GetComponent<Interactor>();
        }

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }



        // =====================================================================
        //
        //                          Event Methods
        //
        // =====================================================================

        private void OnInteractAction(EVT_OnPlayerInteractAction evt) {
            if (heldItem != null) {
                if (interactor.GetCurrentInteractable() == null) {
                    DropHeldItem();
                }
            }
        }



        // =====================================================================
        //
        //                              Methods
        //
        // =====================================================================

        /// <summary>
        /// Tries to grab an item. If an item is already held, it's swapped.
        /// </summary>
        /// <param name="grabbableSO"></param>
        /// <param name="grabbable"></param>
        public void TryGrab(GrabbableItemSO grabbableSO, IGrabbable grabbable) {
            if (heldItem != null) {
                DropHeldItem();
            }

            this.grabbable = grabbable;
            heldItem = grabbableSO;

            grabbable.OnGrab();
            objectHeld = PoolRuntimeSystem.Instance.SpawnFromPool(grabbableSO.nonRB_poolItem.name, handPoint.position, handPoint.rotation, handPoint);
        }


        /// <summary>
        /// Drops the held item
        /// </summary>
        public void DropHeldItem() {
            if (heldItem != null) {
                grabbable.OnDrop(dropPoint.position);
                PoolRuntimeSystem.Instance.ReturnToPool(heldItem.nonRB_poolItem.name, objectHeld);

                heldItem = null;
                grabbable = null;
                objectHeld = null;
            }
        }

        /// <summary>
        /// Checks if an item is currently being held.
        /// </summary>
        public bool IsHoldingItem() {
            return heldItem != null;
        }

        /// <summary>
        /// Gets the Scriptable Object of the currently held item.
        /// </summary>
        public GrabbableItemSO GetHeldItemSO() {
            return heldItem;
        }

        /// <summary>
        /// Consumes the held item (e.g., when placing it in a trash bin).
        /// </summary>
        public void ConsumeHeldItem() {
            if (heldItem != null) {
                PoolRuntimeSystem.Instance.ReturnToPool(heldItem.nonRB_poolItem.name, objectHeld);
                heldItem = null;
                grabbable = null;
                objectHeld = null;
            }
        }
    }
}