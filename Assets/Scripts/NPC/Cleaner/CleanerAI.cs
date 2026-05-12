using AntonLed.StudentAdventure.Core.Audio;
using AntonLed.StudentAdventure.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CleanerAI : MonoBehaviour
{
    // --- НАСТРОЙКИ ---
    private Transform player;

    [Header("Настройки патрулирования")]
    [Tooltip("Радиус, в котором NPC ищет следующую точку для блуждания")]
    public float wanderRadius = 10f;
    [Tooltip("Как далеко от случайной точки можно искать валидное место на NavMesh")]
    public float searchRadius = 1f;

    [Header("Настройки таймеров (в секундах)")]
    [Tooltip("Сколько секунд NPC будет находиться в режиме патрулирования")]
    public float patrolDuration = 120f; // 2 минуты
    [Tooltip("Сколько секунд NPC будет находиться в режиме преследования")]
    public float stalkingDuration = 60f; // 1 минута

    // --- СИСТЕМНЫЕ ПЕРЕМЕННЫЕ ---
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private float currentStateTimer; // Наш таймер-счетчик

    [SerializeField]
    private AudioClip mainMusic;

    [SerializeField]
    private AudioClip stalkingMusic;

    // --- МАШИНА СОСТОЯНИЙ ---
    public enum AIState
    {
        Patrolling,
        Stalking
    }
    private AIState currentState;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("ОШИБКА: Не могу найти объект игрока! Убедитесь, что у него стоит тег 'Player'.");
            this.enabled = false;
        }

        rb = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;

        // Начинаем с патрулирования
        SwitchState(AIState.Patrolling);
    }

    void Update()
    {
        // 1. Первым делом уменьшаем таймер текущего состояния
        currentStateTimer -= Time.deltaTime;

        // 2. Вызываем логику, соответствующую текущему состоянию
        switch (currentState)
        {
            case AIState.Patrolling:
                Update_Patrolling();
                break;
            case AIState.Stalking:
                Update_Stalking();
                break;
        }
    }

    void FixedUpdate()
    {
        if (agent.isStopped || !agent.hasPath)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 1. Получаем от агента направление к следующей точке пути
        Vector2 direction = (agent.steeringTarget - transform.position).normalized;

        // 2. Создаем желаемую скорость
        Vector2 desiredVelocity = direction * agent.speed;

        // 3. Применяем эту скорость к Rigidbody
        rb.linearVelocity = desiredVelocity;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == AIState.Stalking && other.CompareTag("Player"))
        {
            StartCoroutine(PlayerCaught());
        }
    }

    private IEnumerator PlayerCaught()
    {
        Debug.Log("Игрок пойман! Прекращаю преследование и возвращаюсь к патрулированию.");

        float catchMessageDuration = 1.5f;
        WarningUIManager.instance.ShowNotification(WarningType.CleanerCaughtPlayer, catchMessageDuration);
        yield return new WaitForSeconds(catchMessageDuration);


        SwitchState(AIState.Patrolling);

    }

    // Новый, более удобный метод для переключения состояний
    private void SwitchState(AIState newState)
    {
        currentState = newState;

        // При переключении сбрасываем таймер на нужное значение
        switch (currentState)
        {
            case AIState.Patrolling:
                Debug.Log("Начинаю патрулирование на " + patrolDuration + " секунд.");
                WarningUIManager.instance.ShowNotification(WarningType.None);
                MusicManager.instance.SetMusic(mainMusic);
                currentStateTimer = patrolDuration;
                GoToWanderPoint();
                break;
            case AIState.Stalking:
                Debug.Log("Время преследовать! Начинаю охоту на " + stalkingDuration + " секунд.");
                WarningUIManager.instance.ShowNotification(WarningType.CleanerStalking);
                MusicManager.instance.SetMusic(stalkingMusic);
                currentStateTimer = stalkingDuration;
                break;
        }
    }

    // --- ЛОГИКА СОСТОЯНИЙ ---

    void Update_Patrolling()
    {
        // Проверяем условие перехода: таймер вышел?
        if (currentStateTimer <= 0f)
        {
            SwitchState(AIState.Stalking);
            return;
        }

        // Основная логика патрулирования: если дошли, ищем новую точку
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToWanderPoint();
        }
    }

    void Update_Stalking()
    {
        // Проверяем условие перехода: таймер вышел?
        if (currentStateTimer <= 0f)
        {
            SwitchState(AIState.Patrolling);
            return;
        }

        // Основная логика преследования: просто идем к игроку
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---

    void GoToWanderPoint()
    {
        Vector2 pointOnCircle = Random.insideUnitCircle.normalized * wanderRadius;
        Vector2 targetPoint = (Vector2)transform.position + pointOnCircle;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, searchRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}