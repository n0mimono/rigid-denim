using UnityEngine;
using System.Collections;

public class GoKart : VehicleBase {

	private void OnTriggerEnter(Collider other) {
	}

	private void OnTriggerStay(Collider other) {
	}

	protected override void UpdatePosition() {
		// cache update
		position += forward * speed * Time.deltaTime;

		// actual update
		rigid.position = position;
		rigid.rotation = Quaternion.Euler(angles);
	}

}
