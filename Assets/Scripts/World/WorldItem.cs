using AntonLed.StudentAdventure.Core.SceneMenegment;
using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.World
{
    public class WorldItem: MonoBehaviour
    {
        public string uniqueId;

        private void Start()
        {
            if (GameStateManager.instance.collectedItemsIds.Contains(uniqueId))
            {
                Destroy(gameObject);
            }
        }
    }
}