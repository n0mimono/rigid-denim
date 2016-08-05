using UnityEngine;
using System.Collections;
using EasyPhysics;

public class EasyColliderTestHitPoint : MonoBehaviour {
  public EasyCollider src;
  public EasyCollider dst;
  public Transform    res;

  public bool    isHit;
  public Vector3 point;

  void Update() {
    isHit = src.HitTo (dst, out point);
    res.position = point;
  }

}
