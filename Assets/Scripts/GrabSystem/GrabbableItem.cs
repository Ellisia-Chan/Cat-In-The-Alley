using CatInTheAlley.GrabSystem;
using CatInTheAlley.Interfaces;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.SO;
using CatInTheAlley.SoundSystem;
using UnityEngine;

public class GrabbableItem : MonoBehaviour, IInteractable, IGrabbable {

    [Header("Grabbable Item")]
    [SerializeField] private GrabbableItemSO grabbableItemSO;

    [Header("SFX source SO")]
    [SerializeField] private PoolItemSO sfxSourceSO;

    private Transform objectParent;
    private Rigidbody rb;
    private Outline outline;

    private string promptString;

    // =====================================================================
    //
    //                          Unity Lifecycle
    //
    // =====================================================================
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();

    }
    private void OnDisable() {
        if (rb != null) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.identity;
        }
    }


    private void Start() {
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
            interactor.GetComponent<GrabController>()?.TryGrab(grabbableItemSO, sfxSourceSO, this);
        }
    }


    // ---------- IGrabbable Interface ----------

    public GrabbableItemSO GetData() => grabbableItemSO;
    public void OnGrab() {
        if (PoolRuntimeSystem.Instance != null) {
            PoolRuntimeSystem.Instance.ReturnToPool(grabbableItemSO.RB_poolItem.name, gameObject);
        }
    }

    public void OnDrop(Vector3 dropPosition) {
        if (PoolRuntimeSystem.Instance != null) {
            PoolRuntimeSystem.Instance.SpawnFromPool(grabbableItemSO.RB_poolItem.name, dropPosition);
        }
    }
}