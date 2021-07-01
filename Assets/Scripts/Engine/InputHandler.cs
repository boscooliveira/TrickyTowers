using UnityEngine;
using UnityEngine.UI;
using GameProject.TrickyTowers.Controller;
using static UnityEngine.InputSystem.InputAction;

namespace GameProject.TrickyTowers.Engine
{
    [RequireComponent(typeof(PlayerController))]
    public class InputHandler : MonoBehaviour
    {
        private const float REPEAT_INPUT_DELAY = 0.2f;
        private PlayerController _playerController;
        private float _repeatInput = 0;

        public Vector2 InputVector { get; private set; }

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void SetInput(Vector2 input)
        {
            var inputY = InputVector.y;
            InputVector = input;
            if (InputVector.y > 0 && inputY == 0)
                _playerController?.RotatePiece();
            _playerController?.MovePiece(InputVector);
        }

        public void SetInputVector(CallbackContext ctx)
        {
            SetInput(ctx.ReadValue<Vector2>().normalized);
        }

        public void SetInputX(int value)
        {
            _repeatInput = REPEAT_INPUT_DELAY;
            SetInput(Vector2.right * value);
        }

        public void SetInputY(int value)
        {
            SetInput(Vector2.up * value);
        }

        private void Update()
        {
            if (Mathf.Approximately(InputVector.x, 0))
                return;

            if (_repeatInput < 0)
            {
                _repeatInput = REPEAT_INPUT_DELAY;
                SetInput(InputVector);
            }
            _repeatInput -= Time.deltaTime;
        }
    }
}
