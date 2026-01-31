using UnityEngine;

namespace GtaLike.City
{
    [CreateAssetMenu(menuName = "GTA-Like/City/Building Definition")]
    public class BuildingDefinition : ScriptableObject
    {
        public Vector2 footprintSize = new Vector2(6f, 6f);
        public Vector2 heightRange = new Vector2(8f, 30f);
        public Color baseColor = new Color(0.8f, 0.8f, 0.8f);
    }
}
