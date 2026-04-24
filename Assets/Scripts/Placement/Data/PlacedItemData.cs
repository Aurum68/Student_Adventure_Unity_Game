using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.World;
using System.Collections;
using UnityEngine;

namespace AntonLed.StudentAdventure.Placement.Data
{
    public class PlacedItemData
    {
        public ItemData itemData;
        public string uniqueId;
        public Vector2 position;
        public Quaternion rotation;

        public PlacedItemData(ItemData itemData, string uniqueId, Vector2 position, Quaternion rotation)
        {
            this.itemData = itemData;
            this.uniqueId = uniqueId;
            this.position = position;
            this.rotation = rotation;
        }
    }
}