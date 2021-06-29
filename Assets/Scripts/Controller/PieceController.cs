using UnityEngine;  
using GameProject.TrickyTowers.Utils;
using GameProject.TrickyTowers.Config;
using GameProject.TrickyTowers.Controller.PieceState;
using System;

namespace GameProject.TrickyTowers.Controller
{
    public class PieceController : MonoBehaviour, IPoolableItem
    {
        public const float DELAY_BETWEEN_PIECES = 0.7f;
        public const float POSITION_STUCK_TIME = 0.1f;

        public event System.Action<PieceController> OnMoveFinished;
        public event Action<IPoolableItem> OnDisabled;

        [SerializeField]
        private Rigidbody2D _rigidBody;

        [SerializeField]
        private ConstantForce2D _constantForce;

        private Vector3 _initialPosition;
        private Vector3 _lastPosition;
        private float _stuckTime;
        private bool _disableUpdate;
        private bool _disableInput;
        private PlayerAreaBoundaries _bounds;
        private PolygonCollider2D _collider2D;
        private float _rotation;
        private float _delayToDisableUpdate;
        private IPieceFactoryConfig _config;
        private IPhysicsConfig _physicsConfig;
        private bool _wasInited;

        private float _pace { get; set; }

        private IPieceState _beforePlaceState;
        private IPieceState _afterPlaceState;
        private IPieceState _currentState;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void ResetToDefault()
        {
            SetSpeed(_config.SlowPace);
            SetBeforePlacedPhysics();
            _rigidBody.transform.position = _initialPosition;
            _rigidBody.transform.rotation = Quaternion.identity;
            _rotation = _collider2D.transform.eulerAngles.z;
            _lastPosition = _initialPosition;
            _disableUpdate = false;
            _disableInput = false;
            _stuckTime = 0;
            _delayToDisableUpdate = 0;
        }

        public Bounds GetBounds()
        {
            return _collider2D.bounds;
        }

        public void Rotate()
        {
            if (_disableInput || _disableUpdate)
                return;

            _collider2D.transform.Rotate(Vector3.forward, -90f);
            _rotation = _collider2D.transform.eulerAngles.z;
        }

        private void Update()
        {
            if (_rigidBody.transform.position.y <= _bounds.LimitBottom.position.y)
            {
                DestroyPiece();
                return;
            }

            if (_disableUpdate)
                return;

            if (_disableInput)
            {
                _delayToDisableUpdate += Time.deltaTime;
                if (_delayToDisableUpdate >= DELAY_BETWEEN_PIECES)
                {
                    DisableUpdate();
                }
                return;
            }

            if (_collider2D.transform.eulerAngles.z != _rotation)
            {
                DisableInput();
                return;
            }

            if (_rigidBody.transform.position.y < _lastPosition.y)
            {
                _lastPosition = _rigidBody.transform.position;
                _stuckTime = 0;
                return;
            }

            _stuckTime += Time.deltaTime;
            if (_stuckTime >= POSITION_STUCK_TIME)
            {
                DisableInput();
            }
        }

        public void Move(Vector2 input)
        {
            if (_disableInput || _disableUpdate)
                return;

            var position = transform.position;
            position.x += input.x * _config.HorizontalMoveDistance;
            if (input.y < 0)
            {
                if (_pace != _config.FastPace)
                {
                    SetSpeed(_config.FastPace);
                }
            }
            else
            {
                if (_pace != _config.SlowPace)
                {
                    SetSpeed(_config.SlowPace);
                }
            }
            transform.position = position;
        }

        public void SetSpeed(float pace)
        {
            _currentState.SetSpeed(pace);
        }

        private void SetAfterPlacedPhysics()
        {
            _currentState = _afterPlaceState;
            _currentState.Activate();
        }

        private void SetBeforePlacedPhysics()
        {
            _currentState = _beforePlaceState;
            _currentState.Activate();
        }

        private void DisableInput()
        {
            SetAfterPlacedPhysics();
            _delayToDisableUpdate = 0;
            _disableInput = true;
        }

        private void DisableUpdate()
        {
            _disableUpdate = true;
            OnMoveFinished?.Invoke(this);
        }

        private void DestroyPiece()
        {
            gameObject.SetActive(false);
            OnDisabled?.Invoke(this);
        }

        public void SetConfig(IPieceFactoryConfig pieceConfig, IPhysicsConfig physicsConfig, PlayerAreaBoundaries bounds)
        {
            if (!_wasInited)
            {
                _wasInited = true;
                _collider2D = _rigidBody.GetComponent<PolygonCollider2D>();
                _config = pieceConfig;
                _physicsConfig = physicsConfig;

                _rigidBody.sharedMaterial = pieceConfig.PhysicsMaterial2D;
                _collider2D.sharedMaterial = pieceConfig.PhysicsMaterial2D;

                _bounds = bounds;
                _initialPosition = transform.position;
                _lastPosition = _initialPosition;
                _beforePlaceState = new PieceStateImpl(_physicsConfig.BeforePlacedPhysics, _rigidBody, _constantForce);
                _afterPlaceState = new PieceStateImpl(_physicsConfig.AfterPlacedPhysics, _rigidBody, _constantForce);
                _currentState = _beforePlaceState;
            }

            SetBeforePlacedPhysics();
            SetSpeed(_config.SlowPace);
        }

        // Adjust collider bounds to snap into multiples of a Unity`s unit size
        [ContextMenu("PolygonCollider2DSnapHelper")]
        void PolygonColliderSnapHelper()
        {
            PolygonCollider2D collider = GetComponentInChildren<PolygonCollider2D>();
            if (collider == null)
            {
                Debug.LogError("No PolygonCollider2D found");
                return;
            }

            int pathCount = collider.pathCount;
            for (int i = 0; i < pathCount; i++)
            {
                Vector2[] path = collider.GetPath(i);
                for (int j = 0; j < path.Length; j++)
                {
                    Vector2 point = path[j];
                    point.x = (Mathf.Round(point.x / 0.25f) * 0.25f) * 0.98f;
                    point.y = (Mathf.Round(point.y / 0.25f) * 0.25f) * 0.98f;
                    path[j] = point;
                }
                collider.SetPath(i, path);
            }
        }
    }
}