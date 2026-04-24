using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.Utils
{
    public class Utils
    {
        public static void SetLayerRecursive(GameObject gameObject, string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);

            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursive(child.gameObject, layerName);
            }
        }
    }
}
