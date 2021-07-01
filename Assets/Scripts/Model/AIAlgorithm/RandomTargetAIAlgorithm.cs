using UnityEngine;
using System.Collections;

namespace GameProject.TrickyTowers.Model.AIAlgorithm
{
    public class RandomTargetAIAlgorithm : IAIAlgorithm
    {
        private const float MAX_THINKING_TIME = 3f;
        private PolygonCollider2D _collider;
        private float _bestPosX;
        private float _bestRotation;
        private float _minPosX;
        private float _maxPosX;

        public RandomTargetAIAlgorithm(float minPosX, float maxPosX)
        {
            _minPosX = minPosX;
            _maxPosX = maxPosX;
        }

        public void SetPiece(PolygonCollider2D pieceCollider)
        {
            _collider = pieceCollider;
        }

        private void ReCalculateTarget(int iteration, ref int rotationsLeft)
        {
            if (iteration <= rotationsLeft || Random.value > 0.5f)
            {
                _bestRotation += 90;
                rotationsLeft--;
            }
            _bestPosX = Random.Range(_minPosX, _maxPosX);
        }

        public IEnumerator UpdateCoroutine()
        {
            int rotationsLeft = Random.Range(0, 4);

            for (int i = 1; i < 4; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, MAX_THINKING_TIME));
                ReCalculateTarget(i, ref rotationsLeft);
            }
        }

        public Vector2 GetCurrentTarget()
        {
            return new Vector2(_bestPosX, 0);
        }

        public bool GetRotationIntent()
        {
            return _collider.transform.rotation.eulerAngles.z != _bestRotation;
        }

        public Vector2 GetNextMoveIntent()
        {
            return _bestPosX > _collider.transform.position.x ? Vector2.right : Vector2.left;
        }
    }
}
