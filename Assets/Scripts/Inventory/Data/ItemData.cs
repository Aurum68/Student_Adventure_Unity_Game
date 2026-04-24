using UnityEngine;

namespace AntonLed.StudentAdventure.Inventory.Data
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemData : ScriptableObject
    {
        public new string name = "New Item";
        public string description = "Item description";
        public Sprite icon = null;
        public GameObject placementPrefab;
    }
}