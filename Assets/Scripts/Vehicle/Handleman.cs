using UnityEngine;
using System.Collections;

public class Handleman : MonoBehaviour {
	public VehicleBase vehicle;

	[Header("Params")]
	public float forwarding;
	public float circling;
	public float volatility;
	public float radius;
	public float lerpSpeed;

	[Header("State")]
	public float   angle;
	public Vector3 tgtFwd;

	void Update() {
		angle += (2f * Random.value - 1f) * volatility;

		Vector3 fwdPos = vehicle.position + vehicle.forward * forwarding;
		Vector3 offset = Quaternion.AngleAxis(angle, Vector3.up) * vehicle.forward;
		Vector3 target = fwdPos + offset * circling;

		// center-force
		if (vehicle.position.magnitude > radius) {
			tgtFwd = -vehicle.position.normalized;
		} else {
			tgtFwd = (target - vehicle.position).normalized;
		}

		// update
		vehicle.forward = Vector3.Lerp(vehicle.forward, tgtFwd, lerpSpeed * Time.deltaTime).normalized;

	}

}
