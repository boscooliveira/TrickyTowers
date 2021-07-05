using UnityEngine;
using System.Collections;

namespace GameProject.TrickyTowers.Model.AIAlgorithm
{
    public class RandomTargetAI: IAIAlgorithm
    {
        private const float MAX_THINKING_TIME = 3f;
        private PolygonCollider2D _collider;
        private float _bestPosX;
        private float _bestRotation;
        private float _minPosX;
        private float _maxPosX;
        private int _rotationsLeft;

        public RandomTargetAI(float minPosX, float maxPosX)
        {
            _minPosX = minPosX;
            _maxPosX = maxPosX;
        }

        public void SetPiece(PolygonCollider2D pieceCollider)
        {
            _collider = pieceCollider;
        }

        private void ReCalculateTarget(int iteration)
        {
            if (iteration <= _rotationsLeft || Random.value > 0.5f)
            {
                _bestRotation += 90;
                _rotationsLeft--;
            }
            _bestPosX = Random.Range(_minPosX, _maxPosX);
        }

        public IEnumerator UpdateCoroutine()
        {
            _rotationsLeft = Random.Range(0, 4);

            for (int i = 1; i < 4; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, MAX_THINKING_TIME));
                ReCalculateTarget(i);
            }
        }

        public Vector2 GetCurrentTarget()
        {
            return new Vector2(_bestPosX, 0);
        }

        public bool GetRotationIntent()
        {
            return (Mathf.DeltaAngle(_collider.transform.rotation.eulerAngles.z, _bestRotation) < 90);
        }

        public Vector2 GetNextMoveIntent()
        {
            return _bestPosX > _collider.transform.position.x ? Vector2.right : Vector2.left;
        }
    }
}
