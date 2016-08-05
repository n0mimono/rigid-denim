using UnityEngine;
using System.Collections;
using System;
/*
public class ColliderRulerTest : MonoBehaviour {
  public ColliderRuler ruler;

  [Serializable]
  public class Gear {
    public GameObject go;

    public bool       isHit;
    public GameObject other;
    public Vector3    point;
    public float      tstamp;
  }
  public Gear[] gears;

  void Start() {
    MeshCollider[] meshCols = GameObject.FindObjectsOfType<MeshCollider> ();

    gears = new Gear[meshCols.Length];
    for (int i = 0; i < gears.Length; i++) {
      Gear gear = new Gear();
      gear.go = meshCols [i].gameObject;

      meshCols [i].AddCollisionListener ((g, p) => {
        gear.isHit = true;
        gear.other  = g;
        gear.point  = p;
        gear.tstamp = Time.time;
      });

      gears [i] = gear;
    }

    ruler.Initialize ();
  }

  void LateUpdate() {
    for (int i = 0; i < gears.Length; i++) {
      Gear gear = gears [i];
      if (gear.tstamp != Time.time) {
        gear.isHit = false;
        gear.other = null;
        gear.point = Vector3.zero;
        gear.tstamp = Time.time;
      }
    }
  }

}
*/