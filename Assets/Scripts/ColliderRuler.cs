using UnityEngine;
using System.Collections;

public class ColliderRuler : MonoBehaviour {
  public bool initOnStart;
  public bool desableMeshCollider;

  private ColliderHelper[] helpers;

  void Start() {
    if (initOnStart) {
      Initialize ();
    }
  }

  public void Initialize() { 
    MeshCollider[] meshCols = GameObject.FindObjectsOfType<MeshCollider> ();

    helpers = new ColliderHelper[meshCols.Length];
    for (int i = 0; i < helpers.Length; i++) {
      helpers[i] = meshCols [i].InitializeColliderHelper ();
      if (desableMeshCollider) {
        meshCols [i].enabled = false;
      }
    }

  }

  void Update() {

    for (int i = 0; i < helpers.Length; i++) {
      for (int j = 0; j < helpers.Length; j++) {
        if (i == j) continue;

        Vector3 point0, point1;
        bool isHit0 = helpers [i].HitTo (helpers [j], out point0);
        bool isHit1 = helpers [j].HitTo (helpers [i], out point1);

        if (isHit0) {
          helpers [i].InvokeCollisionEvent (helpers [j].gameObject, point0);
        }
        if (isHit1) {
          helpers [j].InvokeCollisionEvent (helpers [i].gameObject, point1);
        }

      }
    }

  }

}
