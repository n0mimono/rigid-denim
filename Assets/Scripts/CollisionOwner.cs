using UnityEngine;
using System.Collections;

public class CollisionOwner : MonoBehaviour {
  public Mesh mesh;
  public Transform trans;

  private Vector3[] vertices;
  private int[] triangles;

  void Start() {
    vertices = mesh.vertices;
    triangles = mesh.triangles;
  }

}
