using UnityEngine;

using CatInTheAlley.EventSystem;
using CatInTheAlley.PlayerSystem.Events;

namespace CatInTheAlley.InputSystem {
    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }


        private InputSystem_Actions inputActions;



        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================

        private void Awake() {
            if (Instance != null) {
                Debug.LogWarning("InputManager: Instance already exists");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Enable();

            inputActions.Player.Interact.performed += OnInteractAction;
        }

        private void OnDisable() {
            inputActions.Disable();

            inputActions.Player.Interact.performed -= OnInteractAction;
        }


        // =====================================================================
        //
        //                          Event Methods
        //
        // =====================================================================

        private void OnInteractAction(UnityEngine.InputSystem.InputAction.CallbackContext context) {
            EventBus.Publish(new EVT_OnPlayerInteractAction());
        }



        // =====================================================================
        //
        //                          Getters and Setters
        //
        // =====================================================================


        /// <summary>
        /// Returns the move vector axis
        /// </summary>
        /// <returns>Vector3</returns>
        public Vector3 GetMoveVectorAxisNormalized() {
            return inputActions.Player.Move.ReadValue<Vector3>().normalized;
        }

        /// <summary>
        /// Returns the look vector axis
        /// </summary>
        /// <returns>Vector2returns>
        public Vector2 GetLookVectorAxis() {
            return inputActions.Player.Look.ReadValue<Vector2>();
        }
    }
}