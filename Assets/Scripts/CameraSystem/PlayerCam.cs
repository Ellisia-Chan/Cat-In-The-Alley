using UnityEngine;
using CatInTheAlley.InputSystem;
using Unity.Cinemachine;

namespace CatInTheAlley.Cameras {
    public class PlayerCam : MonoBehaviour {
        [Header("Camera Settings")]
        [SerializeField] private Transform playerOrientation;
        [SerializeField] private CinemachinePanTilt cinemachinePanTilt;

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            float yaw = cinemachinePanTilt.PanAxis.Value;

            playerOrientation.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}