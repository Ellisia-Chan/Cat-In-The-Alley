using System.Collections;
using UnityEngine;

using CatInTheAlley.SO;
using CatInTheAlley.ObjectPoolSystem;
using CatInTheAlley.ServiceLocator;

namespace CatInTheAlley.SoundSystem {
    public class SFXSource : MonoBehaviour {
        [SerializeField] PoolItemSO sfxSO;
        private AudioSource audioSource;

        // Service Dependencies
        private IPoolService poolService;

        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnDisable() {
            audioSource.clip = null;
            audioSource.Stop();
        }

        private void Start() {
            poolService = ServiceRegistry.Get<IPoolService>();
        }

        private IEnumerator AudioEnded() {
            yield return new WaitWhile(() => audioSource.isPlaying);

            if (poolService != null) {
                poolService.ReturnToPool(sfxSO.name, gameObject);
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