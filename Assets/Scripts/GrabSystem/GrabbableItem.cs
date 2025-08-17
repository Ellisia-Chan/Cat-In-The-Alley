using CatInTheAlley.GrabSystem;
using CatInTheAlley.Interfaces;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.SO;
using CatInTheAlley.SoundSystem;
using UnityEngine;
using System.Collections;

public class GrabbableItem : MonoBehaviour, IInteractable, IGrabbable {

    [Header("Grabbable Item")]
    [SerializeField] private GrabbableItemSO grabbableItemSO;

    [Header("SFX source SO")]
    [SerializeField] private PoolItemSO sfxSourceSO;

    private Transform objectParent;
    private Rigidbody rb;
    private Outline outline;

    private GameObject currentAudioSource;
    private AudioClip lastPlayedClip;

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
            interactor.GetComponent<GrabController>()?.TryGrab(grabbableItemSO, this);
        }
    }


    // ---------- IGrabbable Interface ----------

    public GrabbableItemSO GetData() => grabbableItemSO;
    public void OnGrab() {
        if (PoolRuntimeSystem.Instance != null) {
            PlaySFX(grabbableItemSO.grabSFX);
            PoolRuntimeSystem.Instance.ReturnToPool(grabbableItemSO.RB_poolItem.name, gameObject);
        }
    }

    public void OnDrop(Vector3 dropPosition) {
        if (PoolRuntimeSystem.Instance != null) {
            PoolRuntimeSystem.Instance.SpawnFromPool(grabbableItemSO.RB_poolItem.name, dropPosition);
            PlaySFX(grabbableItemSO.dropSFX);
        }
    }

    // =====================================================================
    //
    //                              Methods
    //
    // =====================================================================

    /// <summary>
    /// Plays an SFX
    /// </summary>
    /// <param name="clip"></param>
    private void PlaySFX(AudioClip clip) {
        if (lastPlayedClip == clip && currentAudioSource != null) {
            AudioSource existingSource = currentAudioSource.GetComponent<AudioSource>();
            if (existingSource != null && existingSource.isPlaying) {
                return;
            }
        }

        currentAudioSource = PoolRuntimeSystem.Instance.SpawnFromPool(sfxSourceSO.name, transform.position);
        SFXSource sfxSource = currentAudioSource.GetComponent<SFXSource>();

        if (sfxSource != null) {
            sfxSource.PlayClip(clip);
            PoolRuntimeSystem.Instance.StartCoroutine(ResetAudioSource(sfxSource));
            lastPlayedClip = clip;
        }
        else {
            Debug.LogWarning("No SFX Source found");
        }
    }

    /// <summary>
    /// Resets the audio source
    /// </summary>
    /// <param name="sfxSource"></param>
    /// <returns></returns>
    private IEnumerator ResetAudioSource(SFXSource sfxSource) {
        yield return new WaitUntil(sfxSource.IsDone);
        currentAudioSource = null;
    }
}