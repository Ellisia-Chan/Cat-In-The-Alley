using UnityEngine;

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
        }

        private void OnDisable() {
            inputActions.Disable();
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
        public Vector3 GetMoveVectorAxis() {
            return inputActions.Player.Move.ReadValue<Vector3>();
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