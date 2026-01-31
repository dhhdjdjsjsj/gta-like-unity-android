using UnityEngine;

namespace GtaLike.City
{
    [CreateAssetMenu(menuName = "GTA-Like/City/District Definition")]
    public class DistrictDefinition : ScriptableObject
    {
        public string districtName = "District";
        public Vector2 buildingDensityRange = new Vector2(0.4f, 0.8f);
        public Vector2 roadWidthRange = new Vector2(4f, 12f);
        public BuildingDefinition[] buildingDefinitions;
        public Color ambientTint = new Color(0.9f, 0.9f, 0.9f);
    }
}
