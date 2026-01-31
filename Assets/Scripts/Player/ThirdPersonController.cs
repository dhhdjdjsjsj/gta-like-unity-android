using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : NetworkBehaviour
    {
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private Transform cameraPivot;

        private CharacterController controller;
        private Vector3 velocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

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

            var camForward = cameraPivot != null ? cameraPivot.forward : Camera.main.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();
            var camRight = cameraPivot != null ? cameraPivot.right : Camera.main.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            var moveDirection = camForward * axis.y + camRight * axis.x;
            var targetSpeed = axis.magnitude > 0.6f ? runSpeed : walkSpeed;
            var move = moveDirection.normalized * targetSpeed;

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                var targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (controller.isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move((move + velocity) * Time.deltaTime);
        }

        public void SetCameraPivot(Transform pivot)
        {
            cameraPivot = pivot;
        }
    }
}
