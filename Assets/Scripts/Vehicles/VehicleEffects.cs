using UnityEngine;

namespace GtaLike.Vehicles
{
    public class VehicleEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem driftDust;
        [SerializeField] private float emissionRate = 20f;

        public void SetSlip(bool isSlipping)
        {
            if (driftDust == null)
            {
                return;
            }

            var emission = driftDust.emission;
            emission.rateOverTime = isSlipping ? emissionRate : 0f;

            if (isSlipping && !driftDust.isPlaying)
            {
                driftDust.Play();
            }
            else if (!isSlipping && driftDust.isPlaying)
            {
                driftDust.Stop();
            }
        }
    }
}
