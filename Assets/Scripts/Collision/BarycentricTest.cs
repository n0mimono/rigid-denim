using UnityEngine;
using System.Collections;

public class BarycentricTest : MonoBehaviour {
  public Transform p0;
  public Transform p1;
  public Transform p2;
  public Transform up;
  public Transform res;
  public Transform resInside;

  public bool isInside;
  public Vector3 lambda;

  void Update() {
    
    Barycentric bc = new Barycentric (
      p0.position,
      p1.position,
      p2.position,
      up.position
    );

    isInside = bc.IsInside;
    lambda = new Vector3 (bc.u, bc.v, bc.w);
    res.position = bc.Interpolate (
      p0.position,
      p1.position,
      p2.position
    );
    resInside.position = bc.InterpolateInside (
      p0.position,
      p1.position,
      p2.position
    );

  }

}
