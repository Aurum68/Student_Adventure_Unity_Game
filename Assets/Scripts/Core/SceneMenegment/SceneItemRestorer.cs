using AntonLed.StudentAdventure.Inventory.Data;
using AntonLed.StudentAdventure.Placement.Data;
using AntonLed.StudentAdventure.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntonLed.StudentAdventure.Core.SceneMenegment
{
    public class SceneItemRestorer : MonoBehaviour
    {
        public List<ItemData> placeableItemPrefabs;

        void Start()
        {
            if (GameStateManager.instance == null) return;

            string sceneName = SceneManager.GetActiveScene().name;

            List<PlacedItemData> itemsToRestore = GameStateManager.instance.placedItems[sceneName];

            Debug.Log($"Найдено {itemsToRestore.Count} предметов для восстановления на сцене.");

            foreach (var itemData in itemsToRestore)
            {
                ItemData prefabData = placeableItemPrefabs.Find(p => p.name == itemData.itemData.name);

                if (prefabData != null && prefabData.placementPrefab != null)
                {
                    // Создаем экземпляр предмета из префаба
                    GameObject newItem = Instantiate(
                        prefabData.placementPrefab,
                        itemData.position,
                        itemData.rotation
                    );

                    WorldItem worldItem = newItem.GetComponent<WorldItem>();
                    if (worldItem != null)
                    {
                        worldItem.uniqueId = itemData.uniqueId;
                    }
                }
                else
                {
                    Debug.LogWarning($"Не найден префаб для предмета с ID: {itemData.itemData.name}");
                }
            }
        }
    }
}