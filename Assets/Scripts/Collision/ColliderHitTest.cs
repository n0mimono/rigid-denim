using UnityEngine;
using System.Collections;

public class ColliderHitTest : MonoBehaviour {
  public Collider src;
  public Collider dst;
  public Transform res;

  public bool    isHit;
  public Vector3 point;

  void Update() {
    isHit = src.HitTo (dst, out point);
    res.position = point;
  }

}
