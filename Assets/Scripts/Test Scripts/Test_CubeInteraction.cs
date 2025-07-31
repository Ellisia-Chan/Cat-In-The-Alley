using UnityEngine;

using CatInTheAlley.Interfaces;
using CatInTheAlley.ObjectPoolSystem;

namespace CatInTheAlley.TestScripts {
    public class Test_CubeInteraction : MonoBehaviour, IInteractable {

        private Outline outline;

        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================

        private void Start() {
            outline = GetComponent<Outline>();
            outline.enabled = false;
        }



        // =====================================================================
        //
        //                          Interface Methods
        //
        // =====================================================================

        // ---------- IInteractable Interface ----------
        public string InteractionPrompt => throw new System.NotImplementedException();

        public void OnFocus() {
            outline.enabled = true;
        }

        public void OnInteract(GameObject interactor) {
            Debug.Log("Interact");
        }

        public void OnLoseFocus() {
            outline.enabled = false;
        }
    }
}
