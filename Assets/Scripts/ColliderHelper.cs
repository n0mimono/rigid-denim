﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ColliderHelper : MonoBehaviour {
  private Transform trans;
  private bool      isMeshCollider;

  private Vector3[] verteces;
  private int[]     triangles;

  private Vector3[] wverts;
  private float[]   distances;
  private List<List<int>> v2t;

  public void Initialize(Collider collider) {
    trans = collider.transform;

    isMeshCollider = (collider.GetType () == typeof(MeshCollider));
    if (isMeshCollider) {
      Mesh mesh = (collider as MeshCollider).sharedMesh;
      verteces  = mesh.vertices;
      triangles = mesh.triangles;
      PreCalculation ();
    }

    hideFlags = ColliderHelperUtility.hideFlags;
  }

  public Vector3 ClosestPointOnVerteces(Vector3 point) {
    if (!isMeshCollider) return trans.position;

    Vector3 pos;
    NearestDistOnVerteces(point, out pos);
    return pos;
  }

  public Vector3 ClosestPointOnMesh(Vector3 point) {
    if (!isMeshCollider) return trans.position;

    Vector3 pos;
    float pointDist = NearestDistOnVerteces (point, out pos);
    return FindNearestPoint (point, pointDist, pos);
  }

  private void PreCalculation() {
    wverts    = new Vector3[verteces.Length];
    distances = new float[verteces.Length];

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

  private float NearestDistOnVerteces(Vector3 point, out Vector3 pos) {
    Matrix4x4 l2w = trans.localToWorldMatrix;

    Vector3 minPoint = trans.position;
    float   minDist  = Vector3.Distance(point, minPoint);
    for (int i = 0; i < verteces.Length; i++) {
      Vector3 v = l2w.MultiplyPoint(verteces [i]);
      float   d = Vector3.Distance (point, v);
      if (d < minDist) {
        minDist  = d;
        minPoint = v;
      }

      wverts [i] = v;
      distances [i] = d;
    }

    pos   = minPoint;
    return minDist;
  }

  private Vector3 FindNearestPoint(Vector3 point, float condDist, Vector3 condPoint) {
    Vector3 minPoint = condPoint;
    float   minDist  = condDist;

    for (int i = 0; i < verteces.Length; i++) {
      float   d = distances[i];
      //if (d > condDist + 1e-5) continue;

      List<int> tris = v2t [i];
      for (int j = 0; j < tris.Count; j++) {
        int triangleIndex = tris [j];
        Vector3 p0 = wverts[triangles [triangleIndex * 3 + 0]];
        Vector3 p1 = wverts[triangles [triangleIndex * 3 + 1]];
        Vector3 p2 = wverts[triangles [triangleIndex * 3 + 2]];
        Barycentric bc = new Barycentric (p0, p1, p2, point);

        Vector3 hitPoint = bc.InterpolateInside(p0, p1, p2);
        float   hitDist  = Vector3.Distance(point, hitPoint);

        if (hitDist < minDist) {
          minPoint = hitPoint;
          minDist  = hitDist;
        }
      }

    }

    return minPoint;
  }

}

public static class ColliderHelperUtility {
  public static HideFlags hideFlags = HideFlags.None;

  public static ColliderHelper InitializeColliderHelper(this Collider collider) {
    ColliderHelper helper = collider.GetComponent<ColliderHelper> ();
    if (helper == null) {
      helper = collider.gameObject.AddComponent<ColliderHelper> ();
    }
    helper.Initialize (collider);

    return helper;
  }

  public static Vector3 ClosestPointOnVerteces(this Collider collider, Vector3 point) {
    ColliderHelper helper = collider.GetComponent<ColliderHelper> ();
    if (helper == null) {
      helper = collider.InitializeColliderHelper ();
    }

    return helper.ClosestPointOnVerteces(point);
  }

  public static Vector3 ClosestPointOnMesh(this Collider collider, Vector3 point) {
    ColliderHelper helper = collider.GetComponent<ColliderHelper> ();
    if (helper == null) {
      helper = collider.InitializeColliderHelper ();
    }

    return helper.ClosestPointOnMesh(point);
  }

}