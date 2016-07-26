using UnityEngine;
using System.Collections;
using System;

public class Vehicle : MonoBehaviour {
	[Header("Rigidbody Management")]
	public Rigidbody rigid;
	public Vector3   position;
	public Vector3   angles;
	public Vector3   forward;

	[Header("Moving")]
	public float speed;
	public float seconds;

	void Start() {
		ForceUpdate();

		this.Update(() => {
			// cache update
			position += forward * speed * Time.deltaTime;

			// actual update
			rigid.MovePosition(position);
			rigid.MoveRotation(Quaternion.Euler(angles));
		}, seconds).StartBy(this);
	}

	private void OnCollisionEnter(Collision collision) {
		ForceUpdate();
	}

	//private void OnTriggerEnter(Collider other) {
	//}

	private void ForceUpdate() {
		position = transform.FreesePosition();
		angles   = transform.FreeseAngles();
		forward  = transform.forward;
	}

}
