using UnityEngine;

using CatInTheAlley.Interfaces;
using CatInTheAlley.GrabSystem;
using CatInTheAlley.ObjectPoolSystem;
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
        if (outline != null) {
            outline.enabled = true; 
        }
    }
    public void OnLoseFocus() {
        if (outline != null) {
            outline.enabled = false; 
        }
    }

    public void OnInteract(GameObject interactor) {
        if (interactor != null) {
            interactor.GetComponent<GrabController>()?.TryGrab(grabbableItemSO, this); 
        }
    }


    // ---------- IGrabbable Interface ----------

    public GrabbableItemSO GetData() => grabbableItemSO;
    public void OnGrab() {
        PoolRuntimeSystem.Instance.ReturnToPool(grabbableItemSO.RB_poolItem.name, gameObject);
    }

    public void OnDrop(Vector3 dropPosition) {
        PoolRuntimeSystem.Instance.SpawnFromPool(grabbableItemSO.RB_poolItem.name, dropPosition);
    }
}