using UnityEngine;
using System.Collections;
/*
public class ColliderRuler : MonoBehaviour {
  public bool initOnStart;
  public bool desableMeshCollider;

  private EasyCollider[] helpers;

  void Start() {
    if (initOnStart) {
      Initialize ();
    }
  }

  public void Initialize() { 
    MeshCollider[] meshCols = GameObject.FindObjectsOfType<MeshCollider> ();

    helpers = new EasyCollider[meshCols.Length];
    for (int i = 0; i < helpers.Length; i++) {
      helpers[i] = meshCols [i].InitializeColliderHelper ();
      if (desableMeshCollider) {
        meshCols [i].enabled = false;
      }
    }

  }

  void Update() {

    for (int i = 0; i < helpers.Length; i++) {
      for (int j = 0; j < i; j++) {
        EasyCollider h0 = helpers [i];
        EasyCollider h1 = helpers [j];
        if (Physics.GetIgnoreLayerCollision(h0.layer, h1.layer)) continue;
        if (h0.HitByAABB(h1)) continue;

        Vector3 point0, point1;
        bool isHit0 = h0.HitTo (h1, out point0);
        bool isHit1 = h1.HitTo (h0, out point1);

        if (isHit0) {
          h0.InvokeCollisionEvent (h0.gameObject, point0);
        }
        if (isHit1) {
          h1.InvokeCollisionEvent (h1.gameObject, point1);
        }

      }
    }

  }

}
*/
