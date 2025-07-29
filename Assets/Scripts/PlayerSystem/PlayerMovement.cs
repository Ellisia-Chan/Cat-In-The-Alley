using UnityEngine;

using CatInTheAlley.InputSystem;

namespace CatInTheAlley.PlayerSystem {
	public class PlayerMovement : MonoBehaviour {


        [Header("Movement")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float groundDrag;

        private float moveSpeed;
        private float horizontalInput;
        private float verticalInput;
        private Vector3 moveDir;

        [Header("Forward Direction")]
        [SerializeField] private Transform orientation;


        private Rigidbody rb;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void Start() {
            rb = GetComponent<Rigidbody>();
            rb.linearDamping = groundDrag;
            moveSpeed = walkSpeed;
        }

        private void Update() {
            HandleInput();
        }

        private void FixedUpdate() {
            HandleMovement();
            SpeedControl();
        }



        // =====================================================================
        //
        //                              Methods
        //
        // =====================================================================

        /// <summary>
        /// Handles player input
        /// </summary>
        private void HandleInput() {
            if (InputManager.Instance != null) {
                Vector3 input = InputManager.Instance.GetMoveVectorAxis();
                horizontalInput = input.x;
                verticalInput = input.y;
            }
        }

        /// <summary>
        /// Handles player movement
        /// </summary>
        private void HandleMovement() {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        /// <summary>
        /// Controls player speed
        /// </summary>
        private void SpeedControl() {
            Vector3 flatvel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (flatvel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatvel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }
}