using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace GameProject.TrickyTowers.Engine
{
    public class InputHandler : MonoBehaviour
    {
        public Vector2 InputVector { get; private set; }

        public void SetInputVector(CallbackContext ctx)
        {
            InputVector = ctx.ReadValue<Vector2>();
        }
    }
}
