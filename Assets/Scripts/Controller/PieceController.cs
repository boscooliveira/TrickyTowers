using UnityEngine;  
using GameProject.TrickyTowers.Utils;
using GameProject.TrickyTowers.Config;
using System;

namespace GameProject.TrickyTowers.Controller
{
    public class PieceController : MonoBehaviour, IPoolableItem
    {
        public const float DELAY_BETWEEN_PIECES = 0.5f;
        public const float POSITION_STUCK_TIME = 1f;

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

        public float Pace { get; private set; }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void ResetToDefault()
        {
            SetSpeed(_config.SlowPace);
            _rigidBody.transform.position = _initialPosition;
            _rigidBody.transform.rotation = Quaternion.identity;
            _rotation = _collider2D.transform.eulerAngles.z;
            _lastPosition = _initialPosition;
            _disableUpdate = false;
            _disableInput = false;
            _stuckTime = 0;
            _delayToDisableUpdate = 0;
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

        public void SetSpeed(float pace)
        {
            Pace = pace;
            _rigidBody.velocity = Vector3.down*10;
            _constantForce.force = new Vector2(0, -_physicsConfig.GravityForce * pace);
        }

        private void DisableInput()
        {
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

        private float GetMinDist(Vector2[] points, Vector2 shift)
        {
            float minHitDist = 0;
            bool noHit = true;

            foreach (Vector2 point in points)
            {
                RaycastHit2D hit = Physics2D.Raycast(point + shift, Vector2.down);
                if (hit.collider != null)
                {
                    if (noHit || hit.distance < minHitDist)
                    {
                        minHitDist = hit.distance;
                        noHit = false;
                    }
                }
            }

            return noHit ? float.NaN : minHitDist;
        }

        private bool GetMinDist(Vector2[] points, Vector2 shift, out int hits, out float maxHitDist)
        {
            maxHitDist = 0;
            hits = 0;
            bool noHit = true;

            foreach (Vector2 point in points)
            {
                RaycastHit2D hit = Physics2D.Raycast(point+shift, Vector2.down);
                if (hit.collider != null)
                {
                    if (noHit || hit.distance > maxHitDist)
                    {
                        hits = 1;
                        maxHitDist = hit.distance;
                        noHit = false;
                    }
                    else if (Mathf.Approximately(hit.distance, maxHitDist))
                    {
                        hits++;
                    }
                }
            }

            return !noHit;
        }

        //public Vector2 GetMoveAssistPosition(Vector2 desiredPos)
        //{
        //    Vector2 newPos = desiredPos;
        //    Vector2 bestPos = desiredPos;
        //    Vector2[] points = _collider2D.points;

        //    for (int i = 0; i < points.Length; i++)
        //    {
        //        var worldPosition = _collider2D.transform.TransformPoint(points[i]);
        //        points[i] = worldPosition;
        //    }

        //    int bestHits;
        //    float bestDist;
        //    GetMinDist(points, newPos - (Vector2)(_collider2D.transform.position), out bestHits, out bestDist);

        //    for (float i = -0.25f; i < 0.25f; i += 0.125f/11)
        //    {
        //        newPos.x = desiredPos.x + i;
        //        int hits;
        //        float dist;
        //        if (!GetMinDist(points, newPos - (Vector2)(_collider2D.transform.position), out hits,out dist))
        //        {
        //            continue;
        //        }

        //        if (bestHits == hits && bestDist < dist)
        //        {
        //            bestHits = hits;
        //            bestDist = dist;
        //            bestPos = newPos;
        //        }
        //    }

        //    return bestPos;
        //}

        public void SetConfig(IPieceFactoryConfig pieceConfig, IPhysicsConfig physicsConfig, PlayerAreaBoundaries bounds)
        {
            _config = pieceConfig;
            _physicsConfig = physicsConfig;
            _rigidBody.angularDrag = physicsConfig.AngularDrag;
            _rigidBody.drag = physicsConfig.LinearDrag;
            _rigidBody.gravityScale = 0;
            _rigidBody.sharedMaterial = pieceConfig.PhysicsMaterial2D;
            _collider2D = _rigidBody.GetComponent<PolygonCollider2D>();
            _collider2D.sharedMaterial = pieceConfig.PhysicsMaterial2D;
            _bounds = bounds;
            _constantForce.force = new Vector2(0, -physicsConfig.GravityForce);
            _initialPosition = _rigidBody.transform.position;
            _lastPosition = _initialPosition;
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