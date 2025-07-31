using UnityEngine;

using System.Collections.Generic;

namespace CatInTheAlley.SO {
	[CreateAssetMenu(fileName = "ObjectSpawnPoint", menuName = "ScriptableObjects/ObjectSpawnPoint")]
	public class ObjectSpawnPointSO : ScriptableObject {
		public PoolItemSO poolItem;
		public List<Vector3> spawnPoints;
	}
}