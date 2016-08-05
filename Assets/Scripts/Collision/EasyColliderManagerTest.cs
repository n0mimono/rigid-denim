using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using EasyPhysics;

public class EasyColliderManagerTest : MonoBehaviour {
  public EasyColliderManager ruler;
  public CollisionDisplay[]  disps;

  IEnumerator Start() {
    ruler.Initialize ();
    yield return new WaitForSeconds (1f);

    foreach (var col in GameObject.FindObjectsOfType<EasyCollider> ()) {
      CollisionDisplay disp = col.gameObject.AddComponent<CollisionDisplay> ();
      col.OnCollision += disp.OnCollision;
    }
  }

}
