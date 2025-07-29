using UnityEngine;

using CatInTheAlley.EventSystem;
using CatInTheAlley.PlayerSystem.Events;
using CatInTheAlley.Interfaces;

namespace CatInTheAlley.PlayerSystem {
    public class Interactor : MonoBehaviour {

        [Header("Interaction")]
        [SerializeField] private Transform interactionSource;
        [SerializeField] private float interactionRange = 2f;

        private IInteractable currentInteractable;
        private IInteractable lastInteractable;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnPlayerInteractAction>(OnInteract);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnPlayerInteractAction>(OnInteract);
        }

        private void Update() {
            CheckForInteractable();
        }





        // =====================================================================
        //
        //                          Event Methods
        //
        // =====================================================================

        /// <summary>
        /// Handles player interaction event
        /// </summary>
        /// <param name="evt"></param>
        private void OnInteract(EVT_OnPlayerInteractAction evt) {
            currentInteractable?.OnInteract(gameObject);
        }





        // =====================================================================
        //
        //                              Methods
        //
        // =====================================================================

        /// <summary>
        /// Checks for interactable objects in front of the player.
        /// </summary>
        private void CheckForInteractable() {
            Ray r = new Ray(interactionSource.position, interactionSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactionRange)) {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                    currentInteractable = interactable;

                    if (currentInteractable != lastInteractable) {
                        lastInteractable?.OnLoseFocus();
                        currentInteractable.OnFocus();
                    }
                }
                else {
                    ClearInteractable();
                }
            }
            else {
                ClearInteractable();
            }

            lastInteractable = currentInteractable;
        }

        /// <summary>
        /// Clears the current interactable if the player is not looking at one.
        /// </summary>
        private void ClearInteractable() {
            if (currentInteractable != null) {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }

        /// <summary>
        /// Draws a gizmo to show the interaction range
        /// </summary>
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(interactionSource.position, interactionSource.forward * interactionRange);
        }
    }
}