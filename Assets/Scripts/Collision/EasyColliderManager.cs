using UnityEngine;
using System.Collections;

namespace EasyPhysics {
  public class EasyColliderManager : MonoBehaviour {
    public bool initOnStart;

    private EasyColliderCluster[] clusters;

    void Start() {
      if (initOnStart) {
        Initialize ();
      }
    }

    public void Initialize() {
      clusters = GameObject.FindObjectsOfType<EasyColliderCluster>();

      foreach (EasyColliderCluster cluster in clusters) {
        cluster.Initialize ();
      }

      StartCoroutine (UpdateCollisions ());
    }

    IEnumerator UpdateCollisions() {
      while (true) {
        CheckClusters ();
        yield return null;
      }
    }

    private void CheckClusters() {
      for (int i = 0; i < clusters.Length; i++) {
        for (int j = 0; j < i; j++) {
          EasyColliderCluster c0 = clusters [i];
          EasyColliderCluster c1 = clusters [j];

          if (EasyPhysics.GetIgnoreLayerCollision(c0.layer, c1.layer)) continue;
          if (!c0.HitByAABB (c1)) continue;

          c0.CheckAndInvokeCollision (c1);
        }
      }

    }

  }

}
