using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyPhysics {

  public class EasyCollider : MonoBehaviour {
    public  bool       initOnStart;

    public  Mesh       mesh   { set; get; }
    public  int        layer  { set; get; }
    public  Transform  trans  { set; get; }
    public  Bounds     bounds { set; get; }
    public  Bounds     worldBounds { get { return new Bounds (trans.position, bounds.size); } }

    private Vector3[]  verteces;
    private int[]      triangles;

    private List<List<int>> v2t;

    private class Link {
      public List<int> joints;
      public List<int> nexts;
      public int       stamp;
      public Vector3   wvert;
      public float     dist2;

      public void Update(Vector3 v, Vector3 point, int ts) {
        if (stamp != ts) {
          wvert = v;
          dist2 = Vector3.SqrMagnitude (point - wvert);
          stamp = ts;
        }
      }

    }
    private List<Link> vlinks;
    private int        timeStamp;

    public delegate void CollisionHandler(EasyCollider other, Vector3 hitPoint);
    public event CollisionHandler OnCollision;

    void Start() {
      if (initOnStart) {
        Initialize ();
      }
    }

    public void Initialize(Mesh mesh, Transform trans, int layer) {
      this.trans = trans;
      this.layer = layer;
     
      verteces   = mesh.vertices;
      triangles  = mesh.triangles;
      bounds     = new Bounds(Vector3.zero, Vector3.one * mesh.bounds.size.Max());

      PreComputeV2T ();
      PreComputeVertexLink ();
    }

    public void Initialize() {
      Initialize (GetComponent<MeshFilter> ().mesh, transform, gameObject.layer);
    }

    private void PreComputeV2T () {
      v2t = new List<List<int>> ();
      for (int i = 0; i < verteces.Length; i++) {
        v2t.Add (new List<int> ());
      }
      for (int j = 0; j < triangles.Length; j++) {
        int vertexIndex = triangles [j];
        int triangleIndex = j / 3;
        v2t [vertexIndex].Add (triangleIndex);
      }
    }

    private void PreComputeVertexLink() {
      vlinks = new List<Link>();
      for (int i = 0; i < verteces.Length; i++) {
        Vector3 v = verteces [i];

        List<int> joints = new List<int>();
        List<int> nexts = new List<int> (); 

        for (int j = 0; j < verteces.Length; j++) {
          if (Vector3.SqrMagnitude (v - verteces [j]) < 1e-2) {
            joints.Add (j);

            List<int> triangleIndeces = v2t [j];
            for (int k = 0; k < triangleIndeces.Count; k++) {
              int triangleIndex = triangleIndeces [k];
              int v0 = triangles [triangleIndex * 3 + 0];
              int v1 = triangles [triangleIndex * 3 + 1];
              int v2 = triangles [triangleIndex * 3 + 2];
              if (!nexts.Contains (v0)) nexts.Add(v0);
              if (!nexts.Contains (v1)) nexts.Add(v1);
              if (!nexts.Contains (v2)) nexts.Add(v2);
            }
          }
        }

        Link link = new Link () {
          joints = joints,
          nexts = nexts
        };
        vlinks.Add (link);
      }
    }

    public Vector3 ClosestPointOnVerteces(Vector3 point) {
      int index = NearestVertex(point, 0);
      return vlinks[index].wvert;
    }

    public Vector3 ClosestPointOnMesh(Vector3 point) {
      int index = NearestVertex(point, 0);
      return FindNearestPoint (point, index);
    }

    private int NearestVertex(Vector3 point, int vertIndex) {
      Matrix4x4 l2w = trans.localToWorldMatrix;

      Link link = vlinks [vertIndex];
      List<int> joints = link.joints;
      List<int> nexts  = link.nexts;

      int   minIndex = 0;
      float minDist2 = 10000000f;

      for (int i = 0; i < joints.Count; i++) {
        int idx = joints [i];
        Link l = vlinks [idx];
        l.Update (l2w.MultiplyPoint (verteces [idx]), point, timeStamp);

        if (l.dist2 < minDist2) {
          minIndex = idx;
          minDist2 = l.dist2;
        }
      }

      for (int i = 0; i < nexts.Count; i++) {
        int idx = nexts [i];
        Link l = vlinks [idx];
        l.Update (l2w.MultiplyPoint (verteces [idx]), point, timeStamp);

        if (l.dist2 < minDist2) {
          minIndex = idx;
          minDist2 = l.dist2;
        }
      }

      for (int i = 0; i < joints.Count; i++) {
        if (joints [i] == minIndex) {
          return joints [i];
        }
      }

      // recursive call
      return NearestVertex(point, minIndex);
    }

    private Vector3 FindNearestPoint(Vector3 point, int vertIndex) {
      Link link = vlinks [vertIndex];

      Vector3 minPoint = link.wvert;
      float   minDist2 = link.dist2;

      List<int> nexts = link.nexts;
      for (int i = 0; i < nexts.Count; i++) {

        // todo: remove triangle overlap
        List<int> tris = v2t [nexts[i]];
        for (int j = 0; j < tris.Count; j++) {
          int triangleIndex = tris [j];
          Vector3 p0 = vlinks[triangles [triangleIndex * 3 + 0]].wvert;
          Vector3 p1 = vlinks[triangles [triangleIndex * 3 + 1]].wvert;
          Vector3 p2 = vlinks[triangles [triangleIndex * 3 + 2]].wvert;
          Barycentric bc = new Barycentric (p0, p1, p2, point);

          Vector3 hitPoint = bc.InterpolateInside(p0, p1, p2);
          float   hitDist  = Vector3.SqrMagnitude(point - hitPoint);

          if (hitDist < minDist2) {
            minPoint = hitPoint;
            minDist2  = hitDist;
          }
        }

      }

      return minPoint;
    }

    public bool HitByAABB(EasyCollider other) {
      return worldBounds.Intersects (other.worldBounds);
    }

    public bool HitTo(EasyCollider other, out Vector3 hitPoint) {
      Vector3 basePoint = trans.position;
      Vector3 revPoint = basePoint;
      Vector3 fwdPoint = Vector3.zero;

      for (int i = 0; i < EasyPhysics.iterationCount; i++) {
        fwdPoint = other.ClosestPointOnMesh (revPoint);
        revPoint = ClosestPointOnMesh (fwdPoint);
      }

      float fwdDist2 = Vector3.SqrMagnitude (basePoint - fwdPoint);
      float revDist2 = Vector3.SqrMagnitude (basePoint - revPoint);
      bool isHit = fwdDist2 < revDist2;

      hitPoint = revPoint;
      return isHit;
    }

    public void BihitAndInvokeCollision(EasyCollider other) {
      Vector3 point0, point1;
      bool isHit0 = HitTo (other, out point0);
      bool isHit1 = other.HitTo (this, out point1);

      if (isHit0 && OnCollision != null) {
        OnCollision (other, point0);
      }
      if (isHit1 && other.OnCollision != null) {
        other.OnCollision (this, point1);
      }

    }

    void LateUpdate() {
      timeStamp++;
    }
  }

}
