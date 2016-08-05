using UnityEngine;
using System.Collections;
using System.Linq;
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

	public static void SetTransform(this Rigidbody rigid, Vector3 position, Vector3 forward) {
		Vector3    pos = position.FreesePosition();
		Quaternion rot = Quaternion.AngleAxis(AngleSigned(Vector3.forward, forward, Vector3.up), Vector3.up);
		if (rigid.isKinematic) {
			rigid.position = pos;
			rigid.rotation = rot;
		} else {
			rigid.MovePosition(pos);
      rigid.MoveRotation(rot);
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
		return Mathf.Atan2(
			Vector3.Dot(n, Vector3.Cross(v1, v2)),
			Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}

	public static string ToString(this object obj, Color color) {
		return string.Format("<color={0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), obj);
	}

	public static void Log(this object obj, object message) {
		Debug.Log(obj + ": " + message);
	}

	public static void CallChild<T>(this Component comp, Action<T> action)  {
		T t = comp.GetComponentInChildren<T>();
		if (t != null) {
			action(t);
		}
	}

	public static void CallChild(this Component comp, string child, Action<GameObject> action) {
		GameObject go = comp.GetComponentsInChildren<Transform>()
			.Select(t => t.gameObject)
			.FirstOrDefault(g => g.name == child);
		if (go != null) {
			action(go);
		}
	}

  public static float Max(this Vector3 vec) {
    return Math.Max (vec.x, Mathf.Max (vec.y, vec.z));
  }

}
