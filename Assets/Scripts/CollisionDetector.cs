using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {
  public Mesh mesh;
  public Transform trans;

  private Vector3[] vertices;

  void Start() {
    vertices = mesh.vertices;

    MeshCollider col = GetComponent<MeshCollider> ();
  }

}
