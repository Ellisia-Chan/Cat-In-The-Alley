using UnityEngine;
using CatInTheAlley.SO;

namespace CatInTheAlley.Interfaces {
	public interface IGrabbable {
		void OnGrab();
		void OnDrop(Vector3 dropPosition);
		GrabbableItemSO GetData();
    }
}