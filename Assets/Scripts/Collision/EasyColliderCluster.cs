using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyPhysics {

  public class EasyColliderCluster : MonoBehaviour {
    public  bool           initOnStart;
    public  bool           initColliders;
    public  bool           replaceMeshCollider;

    public  int            layer { set; get; }

    private EasyCollider[] colliders;
    private Transform      trans;
    private Bounds         bounds;
    private Bounds         worldBounds { get { return new Bounds (trans.position, bounds.size); } }

    void Start() {
      if (initOnStart) {
        Initialize ();
      }
    }

    public void Initialize(EasyCollider[] colliders, Transform trans, int layer) {
      this.colliders = colliders;
      this.trans     = trans;
      this.layer     = layer;

      if (initColliders) {
        foreach (EasyCollider col in colliders) {
          col.Initialize ();
        }
      }

      float ext = colliders.Max (c => {
        Vector3 localPosition = c.trans.position - trans.position;
        float pos = (c.bounds.extents + localPosition).Max();
        float neg = (c.bounds.extents - localPosition).Max();
        return Mathf.Max (pos, neg);
      });
      bounds = new Bounds (Vector3.zero, Vector3.one * ext * 2f);
    }

    public void Initialize() {
      if (replaceMeshCollider) {
        ReplaceMeshCollider ();
      }

      Initialize (GetComponentsInChildren<EasyCollider> (), transform, gameObject.layer);
    }

    public bool HitByAABB(EasyColliderCluster other) {
      return worldBounds.Intersects (other.worldBounds);
    }

    public void CheckAndInvokeCollision(EasyColliderCluster other) {
      EasyCollider[] others = other.colliders;

      for (int i = 0; i < colliders.Length; i++) {
        for (int j = 0; j < others.Length; j++) {
          EasyCollider c0 = colliders [i];
          EasyCollider c1 = others [j];

          if (EasyPhysics.GetIgnoreLayerCollision(c0.layer, c1.layer)) continue;
          if (!c0.HitByAABB(c1)) continue;

          c0.BihitAndInvokeCollision (c1);
        }
      }
    }

    public void ReplaceMeshCollider() {
      MeshCollider[] meshCols = GetComponentsInChildren<MeshCollider> ();
      foreach (MeshCollider meshCol in meshCols) {
        meshCol.gameObject.AddComponent<EasyCollider> ();
        meshCol.enabled = false;
      }
    }

  }

}

