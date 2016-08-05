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

    private Vector3[]  wverts;
    private float[]    distances2;
    private List<List<int>> v2t;

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

      wverts     = new Vector3[verteces.Length];
      distances2 = new float[verteces.Length];

      // pre calculations
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

    public void Initialize() {
      Initialize (GetComponent<MeshFilter> ().mesh, transform, gameObject.layer);
    }

    public Vector3 ClosestPointOnVerteces(Vector3 point) {
      Vector3 pos;
      NearestDistOnVerteces(point, out pos);
      return pos;
    }

    public Vector3 ClosestPointOnMesh(Vector3 point) {
      Vector3 pos;
      float pointDist2 = NearestDistOnVerteces (point, out pos);
      return FindNearestPoint (point, pointDist2, pos);
    }

    private float NearestDistOnVerteces(Vector3 point, out Vector3 pos) {
      Matrix4x4 l2w = trans.localToWorldMatrix;

      Vector3 minPoint = Vector3.zero;
      float   minDist2 = 10000000f;
      for (int i = 0; i < verteces.Length; i++) {
        Vector3 v  = l2w.MultiplyPoint(verteces [i]);
        float   d2 = Vector3.SqrMagnitude (point - v);
        if (d2 < minDist2) {
          minDist2 = d2;
          minPoint = v;
        }

        wverts [i] = v;
        distances2 [i] = d2;
      }

      pos   = minPoint;
      return minDist2;
    }

    private Vector3 FindNearestPoint(Vector3 point, float condDist2, Vector3 condPoint) {
      Vector3 minPoint = condPoint;
      float   minDist2 = condDist2;

      for (int i = 0; i < verteces.Length; i++) {
        float d2 = distances2[i];
        if (d2 > condDist2 + 1e-5) continue;

        List<int> tris = v2t [i];
        for (int j = 0; j < tris.Count; j++) {
          int triangleIndex = tris [j];
          Vector3 p0 = wverts[triangles [triangleIndex * 3 + 0]];
          Vector3 p1 = wverts[triangles [triangleIndex * 3 + 1]];
          Vector3 p2 = wverts[triangles [triangleIndex * 3 + 2]];
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

  }

}
