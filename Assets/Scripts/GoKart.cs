using UnityEngine;
using System.Collections;

public class GoKart : VehicleBase {
	[Header("GoKart")]
	public  float   backScale;
	public  float   kickScale;
	public  float   kickDamp;
	public  float   kickEps;

	// state
	private Vector3 mov;
	private bool    hasColllision;

	private Vector3 kick;
	private bool    hasKick;

	private void OnTriggerStay(Collider other) {
		// rollback
		hasColllision = true;

		// kick
		Vector3 hitPoint = other.ClosestPointOnBounds(position);
		kick = (position - hitPoint).normalized * kickScale;
		hasKick = true;
	}

	protected override void UpdatePosition() {
		if (hasColllision) {
			// rollback
			position -= mov * backScale;
			hasColllision = false;
		}

		if (hasKick) {
			// kick
			position += kick * Time.deltaTime;
			// damping
			kick = Vector3.Lerp(kick, Vector3.zero, kickDamp * Time.deltaTime);
			hasKick = kick.magnitude > kickEps;
		} else {
			// cache update
			mov = forward * speed * Time.deltaTime;
			position += mov;
		}

		// actual update
		rigid.SetTransform(position, angles);
	}

}
