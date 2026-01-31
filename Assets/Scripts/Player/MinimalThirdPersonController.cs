using UnityEngine;

namespace GtaLike.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class MinimalThirdPersonController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 2.5f;
        [SerializeField] private float runSpeed = 4.5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Transform cameraPivot;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            MobileInputSource.ResetActions();
        }

        private void HandleMovement()
        {
            var axis = MobileInputSource.MoveAxis;
            if (axis.sqrMagnitude < 0.01f)
            {
                axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            var pivot = cameraPivot != null ? cameraPivot : Camera.main.transform;
            var forward = pivot.forward;
            forward.y = 0f;
            forward.Normalize();
            var right = pivot.right;
            right.y = 0f;
            right.Normalize();

            var moveDirection = forward * axis.y + right * axis.x;
            var speed = MobileInputSource.ActionPressed ? runSpeed : walkSpeed;
            var targetVelocity = moveDirection.normalized * speed;

            var newPosition = rb.position + targetVelocity * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                var smoothedRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(smoothedRotation);
            }
        }

        public void SetCameraPivot(Transform pivot)
        {
            cameraPivot = pivot;
        }
    }
}
