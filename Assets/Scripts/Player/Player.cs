using AntonLed.StudentAdventure.Core.Interactable;
using AntonLed.StudentAdventure.Core.SceneManagement;
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

        private bool _controllerEnabled = true;

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
            SceneManagement.onSceneLoadFinish += EnableController;
            SceneManagement.onSceneLoadStart += DisableController;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManagement.onSceneLoadFinish -= EnableController;
            SceneManagement.onSceneLoadStart -= DisableController;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _rb.linearVelocity = Vector2.zero;

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

        private void EnableController()
        {
            _controllerEnabled = true;
        }

        private void DisableController()
        {
            _controllerEnabled = false;
            _rb.linearVelocity = Vector2.zero;
            _move = Vector2.zero;

            if (_animator != null)
            {
                UpdateAnimatorIdleState();
            }
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
            if (_controllerEnabled)
            {
                _rb.linearVelocity = _move * _speed;
                return;
            }

            _rb.linearVelocity = Vector2.zero;
        }

        private void Update()
        {
            if (_controllerEnabled == false) return;

            if (_move.sqrMagnitude > 0.1f)
            {
                _animator.SetBool("isMoving", true);
                _lastMoveDirection = _move.normalized;

                _animator.SetFloat("inputX", _move.x);
                _animator.SetFloat("inputY", _move.y);
            }
            else
            {
                UpdateAnimatorIdleState();
            }

            UpdateColliderSize();
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (!_controllerEnabled)
            {
                _move = Vector2.zero;
                return;
            }

            _move = context.ReadValue<Vector2>();
        }

        private void UpdateAnimatorIdleState()
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("lastX", _lastMoveDirection.x);
            _animator.SetFloat("lastY", _lastMoveDirection.y);
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
