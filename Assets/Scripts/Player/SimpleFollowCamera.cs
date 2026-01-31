using UnityEngine;

namespace GtaLike.Player
{
    public class SimpleFollowCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -6f);
        [SerializeField] private float followSpeed = 8f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private string targetTag = "Player";

        private Transform target;

        private void LateUpdate()
        {
            if (target == null)
            {
                var targetObject = GameObject.FindGameObjectWithTag(targetTag);
                if (targetObject != null)
                {
                    target = targetObject.transform;
                }
            }

            if (target == null)
            {
                return;
            }

            var desiredPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            var lookRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
