using System.Collections.Generic;
using UnityEngine;

namespace GtaLike.City
{
    public class CityGenerator : MonoBehaviour
    {
        [Header("City Layout")]
        [SerializeField] private int seed = 42;
        [SerializeField] private Vector2 citySize = new Vector2(800f, 800f);
        [SerializeField] private int mainRoadCount = 6;
        [SerializeField] private int secondaryRoadCount = 20;
        [SerializeField] private float roadSegmentLength = 40f;
        [SerializeField] private DistrictDefinition[] districts;

        [Header("Building")]
        [SerializeField] private int buildingsPerSegment = 6;
        [SerializeField] private float sidewalkWidth = 2f;
        [SerializeField] private Material sharedRoadMaterial;
        [SerializeField] private Material sharedBuildingMaterial;

        private readonly List<GameObject> spawnedObjects = new();
        private System.Random rng;

        public void Generate()
        {
            Clear();

            rng = new System.Random(seed);
            var roadMaterial = sharedRoadMaterial ?? CreateMaterial(new Color(0.15f, 0.15f, 0.15f));
            var buildingMaterial = sharedBuildingMaterial ?? CreateMaterial(new Color(0.7f, 0.7f, 0.7f));

            GenerateRoadNetwork(roadMaterial, buildingMaterial);
        }

        private void Clear()
        {
            foreach (var spawned in spawnedObjects)
            {
                if (spawned != null)
                {
                    DestroyImmediate(spawned);
                }
            }

            spawnedObjects.Clear();
        }

        private void GenerateRoadNetwork(Material roadMaterial, Material buildingMaterial)
        {
            var center = Vector3.zero;
            var majorRoads = CreateCurvedRoads(center, mainRoadCount, 1.5f, roadMaterial);
            var secondaryRoads = CreateCurvedRoads(center, secondaryRoadCount, 0.9f, roadMaterial);

            foreach (var road in majorRoads)
            {
                SpawnBuildingsAlongRoad(road, buildingMaterial, isMajor: true);
            }

            foreach (var road in secondaryRoads)
            {
                SpawnBuildingsAlongRoad(road, buildingMaterial, isMajor: false);
            }
        }

        private List<RoadSegment> CreateCurvedRoads(Vector3 center, int count, float widthMultiplier, Material roadMaterial)
        {
            var segments = new List<RoadSegment>();
            for (int i = 0; i < count; i++)
            {
                var angle = (float)i / count * Mathf.PI * 2f + RandomRange(-0.3f, 0.3f);
                var direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                var pathOrigin = center + direction * RandomRange(50f, 120f);
                var currentPoint = pathOrigin;
                var currentDirection = Quaternion.Euler(0f, RandomRange(-20f, 20f), 0f) * direction;
                var segmentCount = Mathf.RoundToInt(citySize.magnitude / roadSegmentLength * 0.15f) + rng.Next(4, 8);

                for (int s = 0; s < segmentCount; s++)
                {
                    var width = RandomRange(8f, 14f) * widthMultiplier;
                    var nextPoint = currentPoint + currentDirection * roadSegmentLength;
                    var segment = CreateRoadSegment(currentPoint, nextPoint, width, roadMaterial);
                    segments.Add(segment);

                    currentDirection = Quaternion.Euler(0f, RandomRange(-18f, 18f), 0f) * currentDirection;
                    currentPoint = nextPoint;

                    if (Mathf.Abs(currentPoint.x) > citySize.x * 0.5f || Mathf.Abs(currentPoint.z) > citySize.y * 0.5f)
                    {
                        break;
                    }
                }
            }

            return segments;
        }

        private RoadSegment CreateRoadSegment(Vector3 start, Vector3 end, float width, Material roadMaterial)
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "RoadSegment";
            var segmentVector = end - start;
            var length = segmentVector.magnitude;
            road.transform.position = start + segmentVector * 0.5f;
            road.transform.rotation = Quaternion.LookRotation(segmentVector.normalized, Vector3.up);
            road.transform.localScale = new Vector3(width, 0.2f, length);

            var renderer = road.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = roadMaterial;
            renderer.sharedMaterial.enableInstancing = true;

            spawnedObjects.Add(road);

            var sidewalkLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sidewalkLeft.name = "SidewalkLeft";
            sidewalkLeft.transform.SetParent(road.transform, false);
            sidewalkLeft.transform.localPosition = new Vector3(-(width * 0.5f + sidewalkWidth * 0.5f), 0.15f, 0f);
            sidewalkLeft.transform.localScale = new Vector3(sidewalkWidth, 0.1f, length);
            sidewalkLeft.GetComponent<MeshRenderer>().sharedMaterial = CreateMaterial(new Color(0.4f, 0.4f, 0.4f));
            spawnedObjects.Add(sidewalkLeft);

            var sidewalkRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sidewalkRight.name = "SidewalkRight";
            sidewalkRight.transform.SetParent(road.transform, false);
            sidewalkRight.transform.localPosition = new Vector3(width * 0.5f + sidewalkWidth * 0.5f, 0.15f, 0f);
            sidewalkRight.transform.localScale = new Vector3(sidewalkWidth, 0.1f, length);
            sidewalkRight.GetComponent<MeshRenderer>().sharedMaterial = CreateMaterial(new Color(0.4f, 0.4f, 0.4f));
            spawnedObjects.Add(sidewalkRight);

            return new RoadSegment(start, end, width, length);
        }

        private void SpawnBuildingsAlongRoad(RoadSegment road, Material buildingMaterial, bool isMajor)
        {
            var density = isMajor ? 0.75f : 0.55f;
            var buildingCount = Mathf.RoundToInt(buildingsPerSegment * density);
            for (int i = 0; i < buildingCount; i++)
            {
                var t = (i + 0.5f) / buildingCount;
                var position = Vector3.Lerp(road.Start, road.End, t);
                var offsetSide = rng.NextDouble() > 0.5 ? 1f : -1f;
                var lateral = Vector3.Cross((road.End - road.Start).normalized, Vector3.up) * (road.Width * 0.5f + sidewalkWidth + RandomRange(2f, 6f)) * offsetSide;
                var buildingPosition = position + lateral;

                SpawnBuilding(buildingPosition, buildingMaterial);
            }
        }

        private void SpawnBuilding(Vector3 position, Material buildingMaterial)
        {
            var definition = GetRandomBuildingDefinition();
            var footprint = definition != null ? definition.footprintSize : new Vector2(RandomRange(6f, 12f), RandomRange(6f, 12f));
            var height = definition != null ? RandomRange(definition.heightRange.x, definition.heightRange.y) : RandomRange(8f, 28f);
            var baseColor = definition != null ? definition.baseColor : new Color(RandomRange(0.5f, 0.9f), RandomRange(0.5f, 0.9f), RandomRange(0.5f, 0.9f));

            var buildingRoot = new GameObject("Building");
            buildingRoot.transform.position = position;
            buildingRoot.transform.rotation = Quaternion.Euler(0f, RandomRange(0f, 360f), 0f);

            var lodGroup = buildingRoot.AddComponent<LODGroup>();
            var lods = new List<LOD>();

            lods.Add(CreateBuildingLod(buildingRoot.transform, footprint, height, baseColor, buildingMaterial, 0.6f, "LOD0"));
            lods.Add(CreateBuildingLod(buildingRoot.transform, footprint * 0.7f, height * 0.6f, baseColor * 0.9f, buildingMaterial, 0.3f, "LOD1"));
            lods.Add(CreateBuildingLod(buildingRoot.transform, footprint * 0.4f, height * 0.3f, baseColor * 0.8f, buildingMaterial, 0.05f, "LOD2"));

            lodGroup.SetLODs(lods.ToArray());
            lodGroup.RecalculateBounds();

            var collider = buildingRoot.AddComponent<BoxCollider>();
            collider.size = new Vector3(footprint.x, height, footprint.y);
            collider.center = new Vector3(0f, height * 0.5f, 0f);

            spawnedObjects.Add(buildingRoot);
        }

        private LOD CreateBuildingLod(Transform parent, Vector2 footprint, float height, Color color, Material baseMaterial, float screenRelativeHeight, string name)
        {
            var lodObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lodObject.name = name;
            lodObject.transform.SetParent(parent, false);
            lodObject.transform.localPosition = new Vector3(0f, height * 0.5f, 0f);
            lodObject.transform.localScale = new Vector3(footprint.x, height, footprint.y);
            var renderer = lodObject.GetComponent<MeshRenderer>();
            var materialInstance = new Material(baseMaterial) { color = color };
            materialInstance.enableInstancing = true;
            renderer.sharedMaterial = materialInstance;

            return new LOD(screenRelativeHeight, new[] { renderer });
        }

        private Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { color = color };
            material.enableInstancing = true;
            return material;
        }

        private BuildingDefinition GetRandomBuildingDefinition()
        {
            if (districts == null || districts.Length == 0)
            {
                return null;
            }

            var district = districts[rng.Next(0, districts.Length)];
            if (district == null || district.buildingDefinitions == null || district.buildingDefinitions.Length == 0)
            {
                return null;
            }

            return district.buildingDefinitions[rng.Next(0, district.buildingDefinitions.Length)];
        }

        private float RandomRange(float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }

        private readonly struct RoadSegment
        {
            public RoadSegment(Vector3 start, Vector3 end, float width, float length)
            {
                Start = start;
                End = end;
                Width = width;
                Length = length;
            }

            public Vector3 Start { get; }
            public Vector3 End { get; }
            public float Width { get; }
            public float Length { get; }
        }
    }
}
