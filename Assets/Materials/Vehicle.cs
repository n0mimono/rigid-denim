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

public static class Utility {

	public static IEnumerator Update(this object obj, Action action, float seconds = 0f) {
		WaitForSeconds waitSecs = new WaitForSeconds(seconds);

		while (true) {
			action();
			yield return waitSecs;
		}
	}

	public static void StartBy(this IEnumerator routine, MonoBehaviour behav) {
		behav.StartCoroutine(routine);
	}

	public static Vector3 FreesePosition(this Transform trans) {
		return new Vector3(trans.position.x, 0f, trans.position.z);
	}

	public static Vector3 FreeseAngles(this Transform trans) {
		return new Vector3(0f, trans.eulerAngles.z, 0f);
	}

}
