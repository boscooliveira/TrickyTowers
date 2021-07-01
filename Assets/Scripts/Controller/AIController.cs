using UnityEngine;
using System.Collections;
using GameProject.TrickyTowers.Model.AIAlgorithm;

namespace GameProject.TrickyTowers.Controller
{

    [RequireComponent(typeof(PlayerController))]
    public class AIController : MonoBehaviour
    {
        private const float INPUT_PER_SECOND = 10f;

        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private Transform _spawn;

        private PolygonCollider2D _collider;
        private float _inputWait = 0;
        private float _moveDistance;
        private IAIAlgorithm _aiAlgorithm;

        private void Start()
        {
            var service = Service.ServiceFactory.Instance.Resolve<Service.IGameConfigService>();
            _moveDistance = service.PieceConfig.HorizontalMoveDistance;
            _playerController = GetComponent<PlayerController>();
            _playerController.OnPieceChanged += PieceChanged;
            var piece = _playerController.GetPiece();
            if (piece != null)
                PieceChanged(piece);
        }

        private void PieceChanged(PieceController piece)
        {
            _collider = piece.GetComponentInChildren<PolygonCollider2D>();
            _aiAlgorithm.SetPiece(_collider);
            StartCoroutine(_aiAlgorithm.UpdateCoroutine());
        }

        public void SetAlgorithm(IAIAlgorithm aiAlgorithm)
        {
            _aiAlgorithm = aiAlgorithm;
        }

        private void Update()
        {
            _inputWait -= Time.deltaTime;
            if (_collider == null || _inputWait > 0)
                return;

            _inputWait = 1 / INPUT_PER_SECOND;

            if (Mathf.Abs(_collider.transform.position.x - _aiAlgorithm.GetCurrentTarget().x) > _moveDistance)
            {
                _playerController.MovePiece(_aiAlgorithm.GetNextMoveIntent());
            }
        }
    }
}
