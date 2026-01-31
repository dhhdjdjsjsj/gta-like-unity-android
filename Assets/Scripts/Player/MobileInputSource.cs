using UnityEngine;

namespace GtaLike.Player
{
    public static class MobileInputSource
    {
        public static Vector2 MoveAxis { get; private set; }
        public static bool ActionPressed { get; private set; }
        public static bool FirePressed { get; private set; }

        public static void SetMoveAxis(Vector2 axis)
        {
            MoveAxis = Vector2.ClampMagnitude(axis, 1f);
        }

        public static void SetAction(bool pressed)
        {
            ActionPressed = pressed;
        }

        public static void SetFire(bool pressed)
        {
            FirePressed = pressed;
        }

        public static void ResetActions()
        {
            ActionPressed = false;
            FirePressed = false;
        }
    }
}
