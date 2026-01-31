using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Vehicles
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : NetworkBehaviour
    {
        [SerializeField] private WheelCollider[] wheelColliders;
        [SerializeField] private Transform[] wheelMeshes;
        [SerializeField] private float motorTorque = 1500f;
        [SerializeField] private float maxSteerAngle = 28f;
        [SerializeField] private float brakeTorque = 2500f;
        [SerializeField] private VehicleEffects effects;

        private Rigidbody rb;
        private float currentSteer;
        private float currentMotor;
        private bool isBraking;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        }

        public void SetInput(float steering, float throttle, bool brake)
        {
            if (!IsOwner)
            {
                return;
            }

            currentSteer = steering;
            currentMotor = throttle;
            isBraking = brake;
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            ApplyPhysics();
            UpdateWheelVisuals();
        }

        private void ApplyPhysics()
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                var collider = wheelColliders[i];
                var steer = i < 2 ? currentSteer * maxSteerAngle : 0f;
                collider.steerAngle = steer;
                collider.motorTorque = currentMotor * motorTorque;
                collider.brakeTorque = isBraking ? brakeTorque : 0f;
            }

            if (effects != null)
            {
                var slip = Mathf.Abs(currentMotor) > 0.8f && rb.velocity.magnitude < 5f;
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
