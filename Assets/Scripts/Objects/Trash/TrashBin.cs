
using UnityEngine;
using CatInTheAlley.Interfaces;
using CatInTheAlley.GrabSystem;
using CatInTheAlley.SO;

public class TrashBin : MonoBehaviour, IInteractable {

    [Header("Interaction")]
    public string prompt = "Throw Away";

    // You could add SFX or VFX here for feedback
    // [SerializeField] private AudioClip successSound;

    public string InteractionPrompt => prompt;

    public void OnFocus() {
        // Optional: Add highlighting to the trash bin
    }

    public void OnLoseFocus() {
        // Optional: Remove highlighting
    }

    public void OnInteract(GameObject interactor) {
        // 1. Get the player's GrabController
        var grabController = interactor.GetComponent<GrabController>();
        if (grabController == null || !grabController.IsHoldingItem()) {
            // Nothing to do if the interactor can't grab or isn't holding anything
            return;
        }

        // 2. Get the Scriptable Object of the held item to check its properties
        GrabbableItemSO heldItem = grabController.GetHeldItemSO();

        // 3. Check if the item is trashable (requires adding a bool to your SO)
        if (heldItem != null && heldItem.isTrashable) {
            // 4. If it is, consume the item
            grabController.ConsumeHeldItem();

            // 5. Play feedback (e.g., a sound effect)
            // AudioSource.PlayClipAtPoint(successSound, transform.position);
            Debug.Log("Item thrown in the trash!");
        }
        else {
            // Optional: Play a "cannot do that" sound
            Debug.Log("This item cannot be thrown away.");
        }
    }
}
