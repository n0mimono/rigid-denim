using UnityEngine;
using System.Collections;
using EasyPhysics;

public class EasyColliderTestClosestPoint : MonoBehaviour {
  public EasyCollider target;
  public Transform    src;
  public Transform    result;

  public bool useMesh;

  void Update() {
    if (useMesh) {
      result.position = target.ClosestPointOnMesh (src.position);
    } else {
      result.position = target.ClosestPointOnVerteces (src.position);
    }
  }

}
