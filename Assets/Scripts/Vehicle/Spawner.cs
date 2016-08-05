using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public VehicleBase origin;
	public int         num;
	public float       radius;

	IEnumerator Start() {
		
		for (int i = 0; i < num; i++) {
			VehicleBase clone = Instantiate(origin);
			clone.CallChild("Main Camera", g => g.SetActive(false));
			clone.CallChild<Transform>(t => t.SetParent(transform));
			yield return null;

			Vector3 pos = Random.insideUnitSphere * radius;
			Vector3 fwd = Random.insideUnitSphere;
			yield return null;

			clone.position = pos.FreesePosition();
			clone.forward  = fwd.FreesePosition().normalized;
			yield return null;
		}

	}

}
