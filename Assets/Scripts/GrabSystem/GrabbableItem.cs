using UnityEngine;

using CatInTheAlley.Interfaces;
using CatInTheAlley.GrabSystem;
using CatInTheAlley.SO;

public class GrabbableItem : MonoBehaviour, IInteractable, IGrabbable {

    [Header("Grabbable Item")]
    [SerializeField] private GrabbableItemSO grabbableItemSO;

    private Transform objectParent;
    private Rigidbody rb;
    private Outline outline;

    private string promptString;

    // =====================================================================
    //
    //                          Unity Lifecycle
    //
    // =====================================================================

    private void Start() {
        rb = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        outline.enabled = false;

        objectParent = transform.parent;
    }

    // =====================================================================
    //
    //                          Interface Methods
    //
    // =====================================================================

    // ---------- IInteractable Interface ----------

    public string InteractionPrompt => "Grab " + grabbableItemSO.itemName;

    public void OnFocus() {
        outline.enabled = true;
    }
    public void OnLoseFocus() { 
        outline.enabled = false;
    }

    public void OnInteract(GameObject interactor) {
        interactor.GetComponent<GrabController>()?.TryGrab(this);
    }


    // ---------- IGrabbable Interface ----------

    public GrabbableItemSO GetData() => grabbableItemSO;
    public void OnGrab(Transform playerGrabPoint) {

    }

    public void OnDrop(Vector3 dropPosition) {

    }
}