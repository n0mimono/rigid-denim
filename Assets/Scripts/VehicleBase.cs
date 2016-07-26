using UnityEngine;
using System.Collections;

public class VehicleBase : MonoBehaviour {
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
			UpdatePosition();
		}, seconds).StartBy(this);
	}

	protected virtual void UpdatePosition() {
	}

	protected void ForceUpdate() {
		position = transform.FreesePosition();
		angles   = transform.FreeseAngles();
		forward  = transform.forward;
	}

}
