using UnityEngine;

namespace GtaLike.Map
{
    public class MinimapController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float height = 60f;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var position = target.position;
            transform.position = new Vector3(position.x, position.y + height, position.z);
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        }
    }
}
