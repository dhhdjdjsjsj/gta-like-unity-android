using UnityEngine;

namespace GtaLike.Player
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -8f);
        [SerializeField] private float followSpeed = 6f;
        [SerializeField] private float rotationSpeed = 8f;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desiredPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            var lookRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
