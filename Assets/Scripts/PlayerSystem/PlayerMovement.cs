using UnityEngine;

using CatInTheAlley.InputSystem;

namespace CatInTheAlley.PlayerSystem {
    public class PlayerMovement : MonoBehaviour {


        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float groundDrag;

        private float horizontalInput;
        private float verticalInput;
        private Vector3 moveDir;

        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask groundMask;

        private bool isGrounded;

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
            rb.freezeRotation = true;
            rb.linearDamping = groundDrag;
        }

        private void Update() {
            HandleInput();
            GroundCheck();
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
            if (InputManager.Instance == null) return;

            Vector3 input = InputManager.Instance.GetMoveVectorAxisNormalized();
            horizontalInput = input.x;
            verticalInput = input.y;
        }

        /// <summary>
        /// Handles player movement
        /// </summary>
        private void HandleMovement() {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (moveDir.magnitude > 0.1f) {
                // Normal movement
                rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (isGrounded) {
                // Stop sliding when grounded and no input
                Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                Vector3 counterForce = -flatVelocity * 5f; // Adjust 5f to control how strongly it stops
                rb.AddForce(counterForce, ForceMode.Force);
            }
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

        /// <summary>
        /// Checks if the player is on the ground and gets slope information
        /// </summary>
        private void GroundCheck() {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

            if (isGrounded) {
                rb.linearDamping = groundDrag;
            }
            else {
                rb.linearDamping = 0f;
            }
        }
    }
}