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

	public static Vector3 FreesePosition(this Transform trans) {
		return new Vector3(trans.position.x, 0f, trans.position.z);
	}

	public static Vector3 FreeseAngles(this Transform trans) {
		return new Vector3(0f, trans.eulerAngles.z, 0f);
	}

}
