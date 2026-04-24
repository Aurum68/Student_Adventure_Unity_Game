using AntonLed.StudentAdventure.World;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AntonLed.StudentAdventure.Editor
{
    public class SceneItemsProcessor
    {
        // Эта строчка создает новый пункт в верхнем меню Unity
        [MenuItem("Tools/Student Adventure/Assign Unique IDs to Scene Items")]
        private static void AssignUniqueIDs()
        {
            // Находим АБСОЛЮТНО ВСЕ компоненты WorldItem на активной сцене
            WorldItem[] worldItems = Object.FindObjectsByType<WorldItem>(FindObjectsSortMode.None);

            // Создаем список для хранения уже использованных ID
            var usedIDs = worldItems
                .Where(item => !string.IsNullOrEmpty(item.uniqueId))
                .Select(item => item.uniqueId)
                .ToList();

            int assignedCount = 0;
            int issuesCount = 0;

            Debug.Log($"<color=yellow>Начинаем обработку {worldItems.Length} объектов с WorldItem...</color>");

            foreach (WorldItem item in worldItems)
            {
                // === ПРОВЕРКА 1: Объект без ID ===
                if (string.IsNullOrEmpty(item.uniqueId))
                {
                    string newId = GenerateUniqueID(usedIDs);
                    item.uniqueId = newId;
                    usedIDs.Add(newId);

                    // Помечаем объект как "измененный", чтобы Unity сохранил изменения
                    EditorUtility.SetDirty(item);
                    assignedCount++;
                    Debug.Log($"Назначен новый ID объекту '{item.gameObject.name}': {newId}");
                    continue;
                }

                // === ПРОВЕРКА 2: Поиск дубликатов ===
                var duplicates = worldItems.Where(i => i.uniqueId == item.uniqueId).ToList();
                if (duplicates.Count > 1)
                {
                    Debug.LogError($"<color=red>Обнаружен дубликат ID '{item.uniqueId}' на объектах: {string.Join(", ", duplicates.Select(d => d.name))}</color>");
                    issuesCount++;
                }
            }

            if (issuesCount > 0)
            {
                Debug.LogError($"<color=red>Проверка завершена. Обнаружено {issuesCount} проблем с дубликатами ID. Пожалуйста, исправьте их вручную.</color>");
            }
            else if (assignedCount > 0)
            {
                Debug.Log($"<color=green>Проверка завершена. Успешно назначено {assignedCount} новых ID. Не забудьте сохранить сцену (Ctrl+S)!</color>");
            }
            else
            {
                Debug.Log("<color=green>Проверка завершена. Все объекты уже имеют уникальные ID. Всё в порядке!</color>");
            }
        }

        private static string GenerateUniqueID(System.Collections.Generic.List<string> usedIDs)
        {
            string newId;
            do
            {
                newId = System.Guid.NewGuid().ToString();
            }
            while (usedIDs.Contains(newId)); // Маловероятно, но на всякий случай проверим

            return newId;
        }
    }
}
