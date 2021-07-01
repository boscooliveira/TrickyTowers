using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameProject.TrickyTowers.Utils
{
    public class ButtonListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private UnityEvent _onPointerDown;

        [SerializeField]
        private UnityEvent _onPointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUp?.Invoke();
        }
    }

}
