using UnityEngine;
using System.Collections;

namespace GameProject.TrickyTowers.Model.AIAlgorithm
{
    public interface IAIAlgorithm
    {
        Vector2 GetCurrentTarget();
        void SetPiece(PolygonCollider2D piece);
        IEnumerator UpdateCoroutine();
        bool GetRotationIntent();
        Vector2 GetNextMoveIntent();
    }
}
