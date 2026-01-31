using UnityEngine;
using UnityEngine.EventSystems;

namespace GtaLike.UI
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform handle;
        [SerializeField] private float radius = 60f;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
            {
                var clamped = Vector2.ClampMagnitude(localPoint, radius);
                handle.anchoredPosition = clamped;
                var axis = clamped / radius;
                Player.MobileInputSource.SetMoveAxis(axis);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            handle.anchoredPosition = Vector2.zero;
            Player.MobileInputSource.SetMoveAxis(Vector2.zero);
        }
    }
}
