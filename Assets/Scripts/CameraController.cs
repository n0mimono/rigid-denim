using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class CameraController : MonoBehaviour, IDragHandler {
	public float dragScale;

	private Transform camTrans;
	private Vector3 Angles {
		set {
			if (camTrans == null) camTrans = Camera.main.transform;
			camTrans.localEulerAngles = value;
		}
		get {
			if (camTrans == null) camTrans = Camera.main.transform;
			return camTrans.localEulerAngles;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		Vector3 ang = Angles;
		ang.y += eventData.delta.x * dragScale;
		Angles = ang;
	}

}
