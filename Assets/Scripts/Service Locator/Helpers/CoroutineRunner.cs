using System.Collections;
using UnityEngine;

namespace CatInTheAlley.ServiceLocator {
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner {
        private void Awake() {
            ServiceRegistry.Register<ICoroutineRunner>(this);
        }

        public Coroutine RunCoroutine(IEnumerator routine) {
            return StartCoroutine(routine);
        }
    }
}