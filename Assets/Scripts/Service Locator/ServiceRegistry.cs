using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A thread-safe, static service locator for providing global access to services.
/// This helps decouple systems and avoids singleton patterns for every manager.
/// It is persistent across scene loads.
/// </summary>
/// <example>
/// <code>
/// // Registration (e.g., in a manager's Awake)
/// ServiceLocator.Register<IGameManager>(this);
///
/// // Retrieval (e.g., in another script)
/// IGameManager gameManager = ServiceLocator.Get<IGameManager>();
/// gameManager.LoadLevel("Level1");
/// </code>
/// </example>
namespace CatInTheAlley.ServiceLocator {
    public static class ServiceRegistry {
        private static readonly object lockObject = new object();
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service with the locator.
        /// </summary>
        /// <typeparam name="T">The interface or class type of the service.</typeparam>
        /// <param name="service">The instance of the service to register.</param>
        public static void Register<T>(T service) {
            var type = typeof(T);
            lock (lockObject) {
                if (services.ContainsKey(type)) {
                    Debug.LogError($"[ServiceLocator] Service of type {type.Name} is already registered.");
                    return;
                }

                services[type] = service;
            }
            Debug.Log($"[ServiceLocator] Service of type {type.Name} registered.");
        }

        /// <summary>
        /// Unregisters a service from the locator.
        /// </summary>
        /// <typeparam name="T">The type of the service to unregister.</typeparam>
        public static void Unregister<T>() {
            var type = typeof(T);
            lock (lockObject) {
                if (!services.ContainsKey(type)) {
                    Debug.LogWarning($"[ServiceLocator] Attempted to unregister a service of type {type.Name} which is not registered.");
                    return;
                }

                services.Remove(type);
            }
            Debug.Log($"[ServiceLocator] Service of type {type.Name} unregistered.");
        }

        /// <summary>
        /// Retrieves a registered service.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>The instance of the requested service.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service is not registered.</exception>
        public static T Get<T>() {
            var type = typeof(T);
            lock (lockObject) {
                if (!services.TryGetValue(type, out var service)) {
                    // In development, failing fast is often best.
                    // In a production build, you might consider a fallback or a less disruptive error.
                    throw new InvalidOperationException($"[ServiceLocator] Service of type {type.Name} not found. Make sure it was registered before being requested.");
                }
                return (T)service;
            }
        }

        /// <summary>
        /// Checks if a service of a given type is registered.
        /// </summary>
        /// <typeparam name="T">The type of the service to check.</typeparam>
        /// <returns>True if the service is registered, false otherwise.</returns>
        public static bool IsRegistered<T>() {
            lock (lockObject) {
                return services.ContainsKey(typeof(T));
            }
        }

        /// <summary>
        /// Clears all registered services. This is particularly useful for editor scripting and testing.
        /// It's automatically called when exiting play mode in the editor.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clear() {
            lock (lockObject) {
                services.Clear();
            }
#if UNITY_EDITOR
            // This log is helpful for development but can be removed in production builds if desired.
            Debug.Log("[ServiceLocator] All services cleared.");
#endif
        }
    } 
}