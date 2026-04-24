using AntonLed.StudentAdventure.Player;
using UnityEngine;
using Cinemachine;

namespace AntonLed.StudentAdventure.ThirdParty
{
    public class CinemachineTargetSetter : MonoBehaviour
    {
        [Tooltip("Перетащи сюда ВСЕ дочерние виртуальные камеры, которыми управляет эта State-Driven Camera")]
        [SerializeField] private CinemachineVirtualCamera[] childVirtualCameras;

        public void OnEnable()
        {
            Debug.Log("<color=blue>КАМЕРА:</color> Начинаю слушать! Подписываюсь на OnPlayerSpawned.", this.gameObject);
            Player.Player.OnPlayerSpawned += SetTargetsForAllCameras;

            if (Player.Player.instance != null)
            {
                Debug.Log("<color=blue>КАМЕРА:</color> О, а игрок уже на сцене! Не буду ждать события, ставлю цель немедленно.");
                SetTargetsForAllCameras(Player.Player.instance.transform);
            }
        }

        public void OnDisable()
        {
            Debug.Log("<color=red>КАМЕРА:</color> Перестаю слушать! Отписываюсь от OnPlayerSpawned.", this.gameObject);
            Player.Player.OnPlayerSpawned -= SetTargetsForAllCameras;
        }

        private void SetTargetsForAllCameras(Transform playerTransform)
        {
            if (childVirtualCameras == null || childVirtualCameras.Length == 0)
            {
                Debug.LogError("В CinemachineTargetSetter не назначены дочерние виртуальные камеры!");
                return;
            }

            Debug.Log($"Получен сигнал о появлении игрока: {playerTransform.name}. Назначаю цели...");

            foreach (CinemachineVirtualCamera vCam in childVirtualCameras)
            {
                if (vCam != null)
                {
                    vCam.Follow = playerTransform;
                }
            }
        }
    }
}
