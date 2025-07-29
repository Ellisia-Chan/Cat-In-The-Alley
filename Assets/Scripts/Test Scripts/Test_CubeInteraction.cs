using UnityEngine;

using CatInTheAlley.Interfaces;

namespace CatInTheAlley.TestScripts {
    public class Test_CubeInteraction : MonoBehaviour, IInteractable {
        public string InteractionPrompt => throw new System.NotImplementedException();

        public void OnFocus() {
            Debug.Log("Focus");
        }

        public void OnInteract(GameObject interactor) {
            Debug.Log("Interact");
        }

        public void OnLoseFocus() {
            Debug.Log("Lose Focus");
        }
    }
}
