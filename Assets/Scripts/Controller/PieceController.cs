using UnityEngine;  
using GameProject.TrickyTowers.Utils;
using GameProject.TrickyTowers.Config;
using System;

namespace GameProject.TrickyTowers.Controller
{
    public class PieceController : MonoBehaviour, IPoolableItem
    {
        public event System.Action<PieceController> OnMoveFinished;
        public event Action<IPoolableItem> OnDisabled;

        [SerializeField]
        private Rigidbody2D _rigidBody;

        [SerializeField]
        private ConstantForce2D _constantForce;

        private Vector3 _initialPosition;
        private Vector3 _lastPosition;
        private float _time;
        private bool _disableUpdate;
        private PlayerAreaBoundaries _bounds;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void ResetToDefault()
        {
            _rigidBody.transform.position = _initialPosition;
            _rigidBody.transform.rotation = Quaternion.identity;
            _lastPosition = _initialPosition;
            _disableUpdate = false;
            _time = 0;
        }

        private void Update()
        {
            if (_disableUpdate)
                return;

            if (_rigidBody.transform.position.y <= _bounds.LimitBottom.position.y)
            {
                DestroyPiece();
            }
            else if (_rigidBody.transform.position.y < _lastPosition.y)
            {
                _lastPosition = _rigidBody.transform.position;
                _time = 0;
            }
            else
            {
                _time += Time.fixedDeltaTime;
                if (_time >= 1)
                {
                    _disableUpdate = true;
                    OnMoveFinished?.Invoke(this);
                }
            }
        }

        private void DestroyPiece()
        {
            gameObject.SetActive(false);
            OnDisabled?.Invoke(this);
        }

        public void SetConfig(IPieceFactoryConfig pieceConfig, IPhysicsConfig physicsConfig, PlayerAreaBoundaries bounds)
        {
            _rigidBody.angularDrag = physicsConfig.AngularDrag;
            _rigidBody.drag = physicsConfig.LinearDrag;
            _rigidBody.gravityScale = 0;
            _rigidBody.sharedMaterial = pieceConfig.PhysicsMaterial2D;
            _rigidBody.GetComponent<Collider2D>().sharedMaterial = pieceConfig.PhysicsMaterial2D;
            _bounds = bounds;
            _constantForce.force = new Vector2(0, -physicsConfig.GravityForce);
            _initialPosition = _rigidBody.transform.position;
            _lastPosition = _initialPosition;
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
                    point.x = Mathf.Round(point.x / 0.25f) * 0.25f;
                    point.y = Mathf.Round(point.y / 0.25f) * 0.25f;
                    path[j] = point;
                }
                collider.SetPath(i, path);
            }
        }
    }
}