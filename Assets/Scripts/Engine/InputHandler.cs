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
            InputVector = ctx.ReadValue<Vector2>();
            _playerController?.MovePiece(InputVector.normalized.x);
        }
    }
}
