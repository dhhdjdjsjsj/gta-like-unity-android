using UnityEngine;
using UnityEngine.EventSystems;

namespace GtaLike.UI
{
    public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ActionType actionType = ActionType.Action;

        public void OnPointerDown(PointerEventData eventData)
        {
            SetPressed(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetPressed(false);
        }

        private void SetPressed(bool pressed)
        {
            switch (actionType)
            {
                case ActionType.Action:
                    Player.MobileInputSource.SetAction(pressed);
                    break;
                case ActionType.Fire:
                    Player.MobileInputSource.SetFire(pressed);
                    break;
            }
        }

        public enum ActionType
        {
            Action,
            Fire
        }
    }
}
