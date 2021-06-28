using System;
using System.Collections;
using GameProject.TrickyTowers.Config;
using UnityEngine;

namespace GameProject.TrickyTowers.Controller
{
    [RequireComponent(typeof(PlayerController))]
    public class AIController : MonoBehaviour
    {
        private const float INPUT_PER_SECOND = 20f;

        [SerializeField]
        private PlayerController _playerController;
        private PieceController _currentPiece;
        private PolygonCollider2D _collider;
        private Vector2 _bestPos;
        private float _inputWait = 0;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _playerController.OnPieceChanged += PieceChanged;
        }

        private void PieceChanged(PieceController piece)
        {
            _currentPiece = piece;
            _collider = piece.GetComponentInChildren<PolygonCollider2D>();
            StartCoroutine(GetPieceTarget(piece));
        }

        private bool GetMaxDist(Vector2[] points, Vector2 shift, out int hits, out float maxHitDist)
        {
            maxHitDist = 0;
            hits = 0;
            bool noHit = true;

            foreach (Vector2 point in points)
            {
                RaycastHit2D hit = Physics2D.Raycast(point + shift, Vector2.down);
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

        public IEnumerator GetMoveAssistPosition(Vector2 currentPosition, Vector2[] points)
        {
            Vector2 newPos = currentPosition;
            _bestPos = currentPosition;

            int bestHits = 0;
            float bestDist = 0;

            const float step = 0.125f / 3;
            for (float i = -5f, cont = 0; i < 5f; cont++, i += step)
            {
                newPos.x = currentPosition.x + i;
                int hits;
                float dist;
                if (!GetMaxDist(points, newPos - currentPosition, out hits, out dist))
                {
                    continue;
                }

                if (bestHits < hits || bestDist < dist)
                {
                    bestHits = hits;
                    bestDist = dist;
                    _bestPos = newPos;
                    Debug.Log($"_bestPos: {_bestPos}");
                }
                if (cont % 5 == 0)
                {
                    Debug.Log("yield");
                    yield return null;
                }
            }
        }

        private Vector2[] GetPoints()
        {
            var points = _collider.points;
            for (int i = 0; i < points.Length; i++)
            {
                var worldPosition = _collider.transform.TransformPoint(points[i]);
                points[i] = worldPosition;
            }
            return points;
        }

        private IEnumerator GetPieceTarget(PieceController piece)
        {
            var position = _collider.transform.position;
            
            Vector2[] points1 = GetPoints();
            _playerController.RotatePiece();
            Vector2[] points2 = GetPoints();
            _playerController.RotatePiece();
            Vector2[] points3 = GetPoints();
            _playerController.RotatePiece();
            Vector2[] points4 = GetPoints();
            _playerController.RotatePiece();

            yield return GetMoveAssistPosition(position, points1);
            Debug.Log("GetPieceTarget completed");
        }

        private void Update()
        {
            _inputWait -= Time.deltaTime;
            if (_collider == null || _inputWait > 0)
                return;

            if (Mathf.Abs(_collider.transform.position.x - _bestPos.x) > 0)
            {
                _inputWait = 1 / INPUT_PER_SECOND;
                Vector2 dir = (_bestPos - (Vector2)_collider.transform.position);
                dir.y = 0;
                dir = dir.normalized;
                _playerController.MovePiece(dir);
                Debug.Log(dir);
            }
        }
    }
}
