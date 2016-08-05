using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using EasyPhysics;

public class EasyColliderManagerTest : MonoBehaviour {
  public EasyColliderManager ruler;
  public CollisionDisplay[]  disps;

  void Start() {
    ruler.Initialize ();

    disps = GameObject
      .FindObjectsOfType<EasyCollider> ()
      .ToList ()
      .Select (c => c.gameObject.AddComponent<CollisionDisplay> ())
      .ToArray ();
  }

}
