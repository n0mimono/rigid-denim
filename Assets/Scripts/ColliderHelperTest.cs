using UnityEngine;
using System.Collections;

public class ColliderHelperTest : MonoBehaviour {
  public Collider  target;
  public Transform src;
  public Transform result;

  public bool useMesh;

  void Update() {
    if (useMesh) {
      result.position = target.ClosestPointOnMesh (src.position);
    } else {
      result.position = target.ClosestPointOnVerteces (src.position);
    }
  }

}
