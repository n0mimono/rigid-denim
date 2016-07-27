using UnityEngine;
using System.Collections;

public class VehicleBase : MonoBehaviour {
	[Header("Rigidbody Management")]
	public Rigidbody rigid;
	public Vector3   position;
	public Vector3   forward;

	[Header("Moving")]
	public float speed;
	public float seconds;

	void Start() {
		ForceUpdate();

		this.Update(() => {
			// cache update
			UpdatePosition();

			// actual update
			rigid.SetTransform(position, forward);

		}, seconds).StartBy(this);
	}

	protected virtual void UpdatePosition() {
	}

	protected void ForceUpdate() {
		position = transform.position;
		forward  = transform.forward;
	}

}
