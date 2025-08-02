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
        [SerializeField] private Transform dropPoint;

        private IGrabbable grabbable;
        private GrabbableItemSO heldItem;
        private PoolItemSO sfxSource;

        private GameObject objectHeld;
        private GameObject currentAudioSource;

        private AudioClip lastPlayedClip;


        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void OnEnable() {
            EventBus.Subscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<EVT_OnPlayerInteractAction>(OnInteractAction);
        }



        // =====================================================================
        //
        //                          Event Methods
        //
        // =====================================================================

        private void OnInteractAction(EVT_OnPlayerInteractAction evt) {
            if (heldItem != null) {
                DropHeldItem(heldItem, sfxSource, grabbable);
            }
        }



        // =====================================================================
        //
        //                              Methods
        //
        // =====================================================================

        /// <summary>
        /// Tries to grab an item
        /// </summary>
        /// <param name="grabbableSO"></param>
        /// <param name="sfxSourceSO"></param>
        /// <param name="grabbable"></param>
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


        /// <summary>
        /// Drops the held item
        /// </summary>
        /// <param name="grabbableSO"></param>
        /// <param name="sfxSourceSO"></param>
        /// <param name="grabbable"></param>
        public void DropHeldItem(GrabbableItemSO grabbableSO, PoolItemSO sfxSourceSO, IGrabbable grabbable) {
            if (heldItem != null) {
                PoolRuntimeSystem.Instance.ReturnToPool(grabbableSO.nonRB_poolItem.name, objectHeld);
                grabbable.OnDrop(dropPoint.position);
                heldItem = null;

                PlaySFX(grabbableSO, sfxSource, grabbableSO.dropSFX);
            }
        }


        /// <summary>
        /// Plays an SFX
        /// </summary>
        /// <param name="grabbableSO"></param>
        /// <param name="sfxSourceSO"></param>
        /// <param name="clip"></param>
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



        /// <summary>
        /// Resets the audio source
        /// </summary>
        /// <param name="sfxSource"></param>
        /// <returns></returns>
        private IEnumerator ResetAudioSource(SFXSource sfxSource) {
            yield return new WaitUntil((sfxSource.IsDone));
            currentAudioSource = null;
        }
    }
}