using CatInTheAlley.EventSystem;
using CatInTheAlley.Interfaces;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.PlayerSystem.Events;
using CatInTheAlley.SoundSystem;
using CatInTheAlley.GrabSystem.Events;
using CatInTheAlley.SO;

using UnityEngine;
using System.Collections;

namespace CatInTheAlley.GrabSystem {
    public class GrabController : MonoBehaviour {
        [Header("Hand Point")]
        [SerializeField] private Transform handPoint;

        private IGrabbable grabbable;
        private GrabbableItemSO heldItem;
        private PoolItemSO sfxSource;

        private GameObject objectHeld;
        private GameObject currentAudioSource;

        private AudioClip lastPlayedClip;

        private void OnEnable() {
            EventBus.Subscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
            EventBus.Subscribe<EVT_OnSFXEnd>(OnSFXEnd);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
            EventBus.Unsubscribe<EVT_OnSFXEnd>(OnSFXEnd);
        }

        private void OnInteractAction(EVT_OnPlayerInteractAction evt) {
            if (heldItem != null) {
                DropHeldItem(heldItem, sfxSource, grabbable);
            }
        }

        private void OnSFXEnd(EVT_OnSFXEnd evt) => currentAudioSource = null;

        public void TryGrab(GrabbableItemSO grabbableSO, PoolItemSO sfxSourceSO, IGrabbable grabbable) {
            if (heldItem == null) {
                this.grabbable = grabbable;
                heldItem = grabbableSO;
                sfxSource = sfxSourceSO;

                grabbable.OnGrab();

                objectHeld = PoolRuntimeSystem.Instance.SpawnFromPool(grabbableSO.nonRB_poolItem.name, handPoint.position, handPoint.rotation, handPoint);
                PlaySFX(grabbableSO, sfxSource, grabbableSO.grabSFX);
            }
            else {
                DropHeldItem(heldItem, sfxSource, grabbable);
            }
        }

        public void DropHeldItem(GrabbableItemSO grabbableSO, PoolItemSO sfxSourceSO, IGrabbable grabbable) {
            if (heldItem != null) {
                PoolRuntimeSystem.Instance.ReturnToPool(grabbableSO.nonRB_poolItem.name, objectHeld);
                grabbable.OnDrop(handPoint.position);
                heldItem = null;

                PlaySFX(grabbableSO, sfxSource, grabbableSO.dropSFX);
            }
        }


        private void PlaySFX(GrabbableItemSO grabbableSO, PoolItemSO sfxSourceSO, AudioClip clip) {
            if (lastPlayedClip == clip && currentAudioSource != null) {
                AudioSource existingSource = currentAudioSource.GetComponent<AudioSource>();
                if (existingSource != null && existingSource.isPlaying) {
                    return;
                }
            }

            currentAudioSource = PoolRuntimeSystem.Instance.SpawnFromPool(sfxSourceSO.name, gameObject.transform.position);
            SFXSource sfxSource = currentAudioSource.GetComponent<SFXSource>();

            if (sfxSource != null) {
                sfxSource.PlayClip(clip);
                StartCoroutine(ResetAudioSource(sfxSource));
                lastPlayedClip = clip;
            }
            else {
                Debug.LogWarning("No SFX Source found");
            }
        }

        private IEnumerator ResetAudioSource(SFXSource sfxSource) {
            yield return new WaitUntil((sfxSource.IsDone));
            currentAudioSource = null;
        }
    }
}