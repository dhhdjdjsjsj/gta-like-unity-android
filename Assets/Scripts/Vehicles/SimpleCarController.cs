using UnityEngine;

namespace GtaLike.Vehicles
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleCarController : MonoBehaviour
    {
        [SerializeField] private WheelCollider[] wheelColliders;
        [SerializeField] private Transform[] wheelMeshes;
        [SerializeField] private float motorTorque = 600f;
        [SerializeField] private float maxSteerAngle = 22f;
        [SerializeField] private float brakeTorque = 1200f;
        [SerializeField] private VehicleEffects effects;

        private Rigidbody rb;
        private bool isActive;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0f, -0.4f, 0f);
        }

        private void FixedUpdate()
        {
            if (!isActive)
            {
                return;
            }

            var axis = Player.MobileInputSource.MoveAxis;
            if (axis.sqrMagnitude < 0.01f)
            {
                axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            var brake = Input.GetKey(KeyCode.Space);
            ApplyPhysics(axis.x, axis.y, brake);
            UpdateWheelVisuals();
        }

        public void SetActive(bool active)
        {
            isActive = active;
        }

        private void ApplyPhysics(float steering, float throttle, bool brake)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                var collider = wheelColliders[i];
                collider.steerAngle = i < 2 ? steering * maxSteerAngle : 0f;
                collider.motorTorque = throttle * motorTorque;
                collider.brakeTorque = brake ? brakeTorque : 0f;
            }

            if (effects != null)
            {
                var slip = Mathf.Abs(throttle) > 0.8f && rb.velocity.magnitude < 4f;
                effects.SetSlip(slip);
            }
        }

        private void UpdateWheelVisuals()
        {
            for (int i = 0; i < wheelColliders.Length && i < wheelMeshes.Length; i++)
            {
                wheelColliders[i].GetWorldPose(out var position, out var rotation);
                wheelMeshes[i].position = position;
                wheelMeshes[i].rotation = rotation;
            }
        }
    }
}
