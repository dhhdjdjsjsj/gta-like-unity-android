using UnityEngine;

namespace GtaLike.UI
{
    public class OnScreenControls : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private float buttonSize = 120f;
        [SerializeField] private float padding = 20f;

        private void OnGUI()
        {
            var moveAxis = Vector2.zero;
            var runPressed = false;

            var leftRect = new Rect(padding, Screen.height - buttonSize - padding, buttonSize, buttonSize);
            var rightRect = new Rect(padding + buttonSize + 10f, Screen.height - buttonSize - padding, buttonSize, buttonSize);
            var upRect = new Rect(padding + buttonSize * 0.5f, Screen.height - buttonSize * 2f - padding - 10f, buttonSize, buttonSize);

            if (GUI.RepeatButton(leftRect, "◀")) moveAxis.x -= 1f;
            if (GUI.RepeatButton(rightRect, "▶")) moveAxis.x += 1f;
            if (GUI.RepeatButton(upRect, "▲")) moveAxis.y += 1f;

            var runRect = new Rect(Screen.width - buttonSize - padding, Screen.height - buttonSize - padding, buttonSize, buttonSize);
            runPressed = GUI.RepeatButton(runRect, "RUN");

            Player.MobileInputSource.SetMoveAxis(moveAxis);
            Player.MobileInputSource.SetAction(runPressed);
        }
    }
}
