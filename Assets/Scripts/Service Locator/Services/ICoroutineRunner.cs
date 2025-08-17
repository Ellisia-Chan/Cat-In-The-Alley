using System.Collections;
using UnityEngine;

namespace CatInTheAlley.ServiceLocator {
    public interface ICoroutineRunner {
        Coroutine RunCoroutine(IEnumerator routine);
    }
}