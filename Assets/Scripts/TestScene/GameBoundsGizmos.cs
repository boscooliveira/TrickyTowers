using UnityEngine;
using GameProject.TrickyTowers.Controller;
using GameProject.TrickyTowers.Config;

namespace GameProject.TrickyTowers.TestScene
{
    public class GameBoundsGizmos : MonoBehaviour
    {
        [SerializeField]
        private PlayerAreaBoundaries _bounds;

#if UNITY_EDITOR
        private float _spawnerDistance;
        private void Start()
        {
            _spawnerDistance = Service.ServiceFactory.Instance.Resolve<IGameplayConfig>().SpawnerMinDistance;
        }

        private void OnDrawGizmos()
        {
            const float lineSize = 100;
            var defaultColor = Gizmos.color;

            var positionLimit = _bounds.SpawnerPosition.position;
            positionLimit.y -= _spawnerDistance;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_bounds.SpawnerPosition.position + Vector3.left * lineSize,
                _bounds.SpawnerPosition.position + Vector3.right * lineSize);
            Gizmos.DrawLine(positionLimit + Vector3.left * lineSize ,
                positionLimit + Vector3.right * lineSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_bounds.LimitBottom.position + Vector3.left * lineSize,
                _bounds.LimitBottom.position + Vector3.right * lineSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_bounds.LimitLeft.position + Vector3.down * lineSize,
                _bounds.LimitLeft.position + Vector3.up * lineSize);
            Gizmos.DrawLine(_bounds.LimitRight.position + Vector3.down * lineSize,
                _bounds.LimitRight.position + Vector3.up * lineSize);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(_bounds.Goal.position + Vector3.left * lineSize,
                _bounds.Goal.position + Vector3.right * lineSize);

            Gizmos.color = defaultColor;
        }
#endif
    }
}
