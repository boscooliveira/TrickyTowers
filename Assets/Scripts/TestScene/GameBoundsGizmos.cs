using UnityEngine;
using GameProject.TrickyTowers.Controller;
using GameProject.TrickyTowers.Service;
using GameProject.TrickyTowers.Config;

namespace GameProject.TrickyTowers.TestScene
{
    public class GameBoundsGizmos : MonoBehaviour
    {
        [SerializeField]
        private PlayerAreaBoundaries _bounds;

#if UNITY_EDITOR
        private IGameplayConfig _config;
        private void Awake()
        {
            _config = ServiceFactory.Instance.Resolve<IGameplayService>().GetGameData().Config;
        }

        private void OnDrawGizmos()
        {
            const float lineSize = 100;
            var defaultColor = Gizmos.color;

            var spawnerPosition = _bounds.SpawnerPosition.position;
            var goalPosition = _bounds.Goal.position;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnerPosition, 3);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_bounds.LimitBottom.position + Vector3.left * lineSize,
                _bounds.LimitBottom.position + Vector3.right * lineSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_bounds.LimitLeft.position + Vector3.down * lineSize,
                _bounds.LimitLeft.position + Vector3.up * lineSize);
            Gizmos.DrawLine(_bounds.LimitRight.position + Vector3.down * lineSize,
                _bounds.LimitRight.position + Vector3.up * lineSize);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(goalPosition + Vector3.left * lineSize,
                goalPosition + Vector3.right * lineSize);

            Gizmos.color = defaultColor;
        }
#endif
    }
}
