using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GtaLike.Systems
{
    public class QualitySettingsManager : MonoBehaviour
    {
        [SerializeField] private UniversalRenderPipelineAsset urpAsset;
        [SerializeField] private Volume postProcessingVolume;
        [SerializeField] private float lowDrawDistance = 200f;
        [SerializeField] private float mediumDrawDistance = 350f;
        [SerializeField] private float highDrawDistance = 500f;

        public void SetQualityLow()
        {
            QualitySettings.SetQualityLevel(0, true);
            ApplyDrawDistance(lowDrawDistance);
        }

        public void SetQualityMedium()
        {
            QualitySettings.SetQualityLevel(1, true);
            ApplyDrawDistance(mediumDrawDistance);
        }

        public void SetQualityHigh()
        {
            QualitySettings.SetQualityLevel(2, true);
            ApplyDrawDistance(highDrawDistance);
        }

        public void ToggleShadows(bool enabled)
        {
            if (urpAsset != null)
            {
                urpAsset.supportsMainLightShadows = enabled;
                urpAsset.supportsAdditionalLightShadows = enabled;
            }
        }

        public void TogglePostProcessing(bool enabled)
        {
            if (postProcessingVolume != null)
            {
                postProcessingVolume.enabled = enabled;
            }
        }

        public void SetDrawDistance(float distance)
        {
            ApplyDrawDistance(distance);
        }

        private void ApplyDrawDistance(float distance)
        {
            QualitySettings.shadowDistance = distance * 0.3f;
            RenderSettings.fogEndDistance = distance;
        }
    }
}
