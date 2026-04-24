using AntonLed.StudentAdventure.Core.Interactable;
using AntonLed.StudentAdventure.Core.SceneMenegment;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace AntonLed.StudentAdventure.Player
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        [SerializeField]
        private float _speed = 3f;

        private Rigidbody2D _rb;

        [SerializeField]
        private Animator _animator;

        private Vector2 _move;

        public static event Action<Transform> OnPlayerSpawned;

        private Vector2 _lastMoveDirection;

        private BoxCollider2D _boxCollider;

        [SerializeField] 
        private Vector2 _verticalSize = new Vector2(1f, 1.5f);

        [SerializeField] 
        private Vector2 _verticalOffset = new Vector2(0f, -0.25f);

        [SerializeField] 
        private Vector2 _horizontalSize = new Vector2(1.5f, 1f);

        [SerializeField] 
        private Vector2 _horizontalOffset = new Vector2(0f, -0.5f);

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;   
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            SpawnPointId targetPointId = SceneTransitionData.targetSpawnPointId;

            Debug.LogWarning($"<color=orange>ИГРОК:</color> Ищу точку '{targetPointId}'.");
            var targetPoint = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None).FirstOrDefault(sp => sp.spawnPointId == targetPointId);

            if (targetPoint == null)
            {
                Debug.LogWarning($"<color=orange>ИГРОК:</color> Не смог найти точку '{targetPointId}'. Ищу точку по умолчанию 'StartGame'...");
                targetPoint = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None).FirstOrDefault(sp => sp.spawnPointId == SpawnPointId.StartGame);
            }

            if (targetPoint != null)
            {
                transform.position = targetPoint.transform.position;
            }
            else
            {
                Debug.LogWarning($"<color=orange>ИГРОК:</color> На сцене '{scene.name}' не найдена точка спавна!");
            }

            SceneTransitionData.targetSpawnPointId = SpawnPointId.StartGame;
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();

            if (OnPlayerSpawned != null)
            {
                Debug.Log("<color=green>ИГРОК:</color> Отправляю сигнал OnPlayerSpawned!", this.gameObject);
                OnPlayerSpawned.Invoke(this.transform);
            }
            else
            {
                Debug.LogWarning("<color=orange>ИГРОК:</color> Хотел отправить сигнал, но меня никто не слушает! (OnPlayerSpawned is null)");
            }

            _lastMoveDirection = new Vector2(0f, -1f);
            UpdateColliderSize();
        }

        void FixedUpdate()
        {
            _rb.linearVelocity = _move * _speed;
        }

        public void Move(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();

            if (_move.sqrMagnitude > 0.1f)
            {
                _animator.SetBool("isMoving", true);
                _lastMoveDirection = _move;
            }
            else
            {
                _animator.SetBool("isMoving", false);
            }

            _animator.SetFloat("inputX", _move.x);
            _animator.SetFloat("inputY", _move.y);

            if (context.canceled)
            {
                _animator.SetFloat("lastX", _lastMoveDirection.x);
                _animator.SetFloat("lastY", _lastMoveDirection.y);
            }

            UpdateColliderSize();
        }

        private void UpdateColliderSize()
        {
            if (Mathf.Abs(_lastMoveDirection.x) >= Mathf.Abs(_lastMoveDirection.y))
            {
                if (_boxCollider.size != _horizontalSize)
                {
                    _boxCollider.size = _horizontalSize;
                    _boxCollider.offset = _horizontalOffset;
                }
            }
            else
            {
                if (_boxCollider.size != _verticalSize)
                {
                    _boxCollider.size = _verticalSize;
                    _boxCollider.offset = _verticalOffset;
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.ToLower().Equals("stairs"))
            {
                collision.gameObject.GetComponent<IInteractable>().Interact();
            }
        }
    }

}
