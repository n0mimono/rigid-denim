using UnityEngine;
using System.Collections;
using System;

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

	public static Vector3 FreesePosition(this Vector3 position) {
		return new Vector3(position.x, 0f, position.z);
	}

	public static Vector3 FreeseAngles(this Vector3 eulerAngles) {
		return new Vector3(0f, eulerAngles.z, 0f);
	}

	public static Vector3 FreesePosition(this Transform trans) {
		return trans.position.FreesePosition();
	}

	public static Vector3 FreeseAngles(this Transform trans) {
		return trans.position.FreeseAngles();
	}

	public static void SetTransform(this Rigidbody rigid, Vector3 position, Vector3 eulerAngles) {
		Vector3    pos = position.FreesePosition();
		Quaternion rot = Quaternion.Euler(eulerAngles.FreeseAngles());
		if (rigid.isKinematic) {
			rigid.position = pos;
			rigid.rotation = rot;
		} else {
			rigid.MovePosition(pos);
			rigid.MoveRotation(rot);
		}
	}

	public static string ToString(this object obj, Color color) {
		return string.Format("<color={0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), obj);
	}

	public static void Log(this object obj, object message) {
		Debug.Log(obj + ": " + message);
	}

}
