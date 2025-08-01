using System.Collections;
using UnityEngine;

using CatInTheAlley.SO;
using CatInTheAlley.ObjectPoolSystem;

namespace CatInTheAlley.SoundSystem {
    public class SFXSource : MonoBehaviour {
        [SerializeField] PoolItemSO sfxSO;
        private AudioSource audioSource;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnDisable() {
            audioSource.clip = null;
            audioSource.Stop();
        }

        private IEnumerator AudioEnded() {
            yield return new WaitWhile(() => audioSource.isPlaying);

            if (PoolRuntimeSystem.Instance != null) { 
                PoolRuntimeSystem.Instance.ReturnToPool(sfxSO.name, gameObject);

            }
        }

        public void PlayClip(AudioClip clip) {
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(AudioEnded());
        }

        public bool IsDone() {
            return !audioSource.isPlaying;
        }
    }
}