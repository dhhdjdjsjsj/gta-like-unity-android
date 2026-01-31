using UnityEngine;

namespace GtaLike.Vehicles
{
    public class VehicleDustBootstrap : MonoBehaviour
    {
        [SerializeField] private VehicleEffects effects;

        private void Awake()
        {
            if (effects == null)
            {
                effects = GetComponent<VehicleEffects>();
            }

            if (effects == null)
            {
                return;
            }

            var dustObject = new GameObject("WheelDust");
            dustObject.transform.SetParent(transform, false);
            dustObject.transform.localPosition = new Vector3(0f, 0.2f, -0.6f);

            var particleSystem = dustObject.AddComponent<ParticleSystem>();
            var main = particleSystem.main;
            main.startLifetime = 0.6f;
            main.startSpeed = 1.2f;
            main.startSize = 0.2f;
            main.loop = true;

            var emission = particleSystem.emission;
            emission.rateOverTime = 0f;

            var shape = particleSystem.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(1.2f, 0.1f, 0.6f);

            effects.SetSlip(false);
            var effectsField = typeof(VehicleEffects).GetField("driftDust", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            effectsField?.SetValue(effects, particleSystem);
        }
    }
}
