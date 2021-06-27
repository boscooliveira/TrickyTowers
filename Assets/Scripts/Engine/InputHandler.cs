using UnityEngine;
using GameProject.TrickyTowers.Controller;
using static UnityEngine.InputSystem.InputAction;

namespace GameProject.TrickyTowers.Engine
{
    [RequireComponent(typeof(PlayerController))]
    public class InputHandler : MonoBehaviour
    {
        private PlayerController _playerController;

        public Vector2 InputVector { get; private set; }

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
        }

        public void SetInputVector(CallbackContext ctx)
        {
            var inputY = InputVector.y;
            InputVector = ctx.ReadValue<Vector2>().normalized;
            if (InputVector.y > 0 && inputY == 0)
                _playerController?.RotatePiece();
            _playerController?.MovePiece(InputVector);
        }

        public void OnFireInput(CallbackContext ctx)
        {
            if (ctx.phase == UnityEngine.InputSystem.InputActionPhase.Started)
            {
                _playerController?.RotatePiece();
            }
        }
    }
}
