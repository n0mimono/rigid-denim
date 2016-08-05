using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace EasyPhysics {
  
  public class EasyPhysics {

    public static int iterationCount = 1;

    public static bool GetIgnoreLayerCollision(int layer0, int layer1) {
      return Physics.GetIgnoreLayerCollision (layer0, layer1);
    }

  }

}
