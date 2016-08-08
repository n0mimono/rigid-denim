using UnityEngine;
using System.Collections;

public class MeshTriggerHelper : MonoBehaviour {
  private Vector3[] verteces;
  private Transform trans;

  void Awake() {
    MeshCollider col = GetComponent<MeshCollider> ();
    verteces = col.sharedMesh.vertices;
    trans = transform;
  }

  public Vector3 ClosestPointOnMesh(Vector3 point) {
    Matrix4x4 l2w = trans.localToWorldMatrix;

    Vector3 minPoint = trans.position;
    float minDist2 = Vector3.SqrMagnitude(minPoint - point);
    for (int i = 0; i < verteces.Length; i++) {
      Vector3 v = l2w.MultiplyPoint (verteces [i]);
      float dist2 = Vector3.SqrMagnitude (v - point);
      if (dist2 < minDist2) {
        minPoint = v;
        minDist2 = dist2;
      }
    }
    return minPoint;
  }

}
