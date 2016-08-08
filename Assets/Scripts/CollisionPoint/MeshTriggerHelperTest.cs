using UnityEngine;
using System.Collections;

public class MeshTriggerHelperTest : MonoBehaviour {
  public Transform point;

  void OnTriggerStay(Collider collider) {
    MeshTriggerHelper helper = collider.GetComponent<MeshTriggerHelper> ();
    if (helper != null) {
      point.position = helper.ClosestPointOnMesh (transform.position);
    }
  }

}
