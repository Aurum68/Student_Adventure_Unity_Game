using AntonLed.StudentAdventure.Core.Audio;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AntonLed.StudentAdventure.UI
{
    public enum WarningType
    {
        None,
        CleanerStalking,
        CleanerCaughtPlayer
    }

    public class WarningUIManager : MonoBehaviour
    {
        public static WarningUIManager instance;

        [Header("Общие UI Элементы")]
        public TextMeshProUGUI fullscreenText;
        public Image persistentIcon;

        [Header("Спрайты для иконок")]
        public Sprite stalkingSprite;

        public AudioClip stalkingSound;
        public AudioClip caughtPlayerSound;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        public void ShowNotification(WarningType type, float duration = 1.5f)
        {
            // Сначала всё выключим для чистоты
            HideAll();

            string text = "";

            switch (type)
            {
                case WarningType.CleanerStalking:
                    text = "БЕРЕГИСЬ УБОРЩИЦУ!";
                    AudioManager.instance.PlaySound(stalkingSound);
                    break;
                case WarningType.CleanerCaughtPlayer:
                    text = "Вас поймали";
                    AudioManager.instance.PlaySound(caughtPlayerSound);
                    break;
                case WarningType.None:
                    return;
            }

            StartCoroutine(ShowAlertAndPersistentIcon(text, duration));
        }

        // Приватный метод, чтобы спрятать все элементы
        public void HideAll()
        {
            StopAllCoroutines(); // Останавливаем все предыдущие анимации
            fullscreenText.gameObject.SetActive(false);
            persistentIcon.gameObject.SetActive(false);
        }

        // Модифицированная корутина для "Тёти Маши"
        private IEnumerator ShowAlertAndPersistentIcon(string message, float duration)
        {
            fullscreenText.text = message;
            fullscreenText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            fullscreenText.gameObject.SetActive(false);

            // Часть 2: Постоянная иконка
            persistentIcon.sprite = stalkingSprite;
            persistentIcon.gameObject.SetActive(true);
        }
    }
}