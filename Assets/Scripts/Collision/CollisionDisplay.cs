using UnityEngine;
using System.Collections;
using EasyPhysics;

public class CollisionDisplay : MonoBehaviour, IEasyCollisionReceiver {
  public bool         isHit;
  public EasyCollider other;
  public Vector3      point;
  public float        tstamp;

  public void OnCollision (EasyCollider other, Vector3 hitPoint) {
    isHit      = true;
    this.other = other;
    this.point = hitPoint;
    tstamp     = Time.time;
  }

  void LateUpdate() {
    if (tstamp != Time.time) {
      isHit      = false;
      this.other = null;
      this.point = Vector3.zero;
      tstamp     = Time.time;
    }
  }

}
