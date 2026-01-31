using System.Collections.Generic;
using UnityEngine;

namespace GtaLike.City
{
    public class LightweightCityGenerator : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private int seed = 5;
        [SerializeField] private int gridSize = 6;
        [SerializeField] private float blockSize = 18f;
        [SerializeField] private float roadWidth = 3f;
        [SerializeField] private Vector2 heightRange = new Vector2(4f, 16f);

        [Header("Rendering")]
        [SerializeField] private Material buildingMaterial;
        [SerializeField] private Material roadMaterial;

        private readonly List<GameObject> spawned = new();
        private System.Random rng;

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            Clear();
            rng = new System.Random(seed);

            var roadMat = roadMaterial ?? CreateMaterial(new Color(0.2f, 0.2f, 0.2f));
            var buildingMat = buildingMaterial ?? CreateMaterial(new Color(0.65f, 0.65f, 0.65f));

            SpawnRoads(roadMat);
            SpawnBuildings(buildingMat);
        }

        private void Clear()
        {
            foreach (var obj in spawned)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }
            spawned.Clear();
        }

        private void SpawnRoads(Material mat)
        {
            var totalSize = gridSize * blockSize;
            var halfSize = totalSize * 0.5f;

            for (int i = 0; i <= gridSize; i++)
            {
                var offset = -halfSize + i * blockSize;
                SpawnRoad(new Vector3(offset, 0f, 0f), new Vector3(offset, 0f, totalSize), mat);
                SpawnRoad(new Vector3(0f, 0f, offset), new Vector3(totalSize, 0f, offset), mat);
            }
        }

        private void SpawnBuildings(Material mat)
        {
            var totalSize = gridSize * blockSize;
            var halfSize = totalSize * 0.5f;

            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    var basePos = new Vector3(-halfSize + x * blockSize + blockSize * 0.5f, 0f, -halfSize + z * blockSize + blockSize * 0.5f);
                    var count = rng.Next(1, 4);
                    for (int i = 0; i < count; i++)
                    {
                        var offset = new Vector3(RandomRange(-blockSize * 0.25f, blockSize * 0.25f), 0f, RandomRange(-blockSize * 0.25f, blockSize * 0.25f));
                        var position = basePos + offset;
                        var footprint = new Vector2(RandomRange(3f, 6f), RandomRange(3f, 6f));
                        var height = RandomRange(heightRange.x, heightRange.y);
                        SpawnBuilding(position, footprint, height, mat);
                    }
                }
            }
        }

        private void SpawnRoad(Vector3 start, Vector3 end, Material mat)
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "Road";
            var dir = end - start;
            var length = dir.magnitude;
            road.transform.position = start + dir * 0.5f;
            road.transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            road.transform.localScale = new Vector3(roadWidth, 0.2f, length + roadWidth);
            var renderer = road.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = mat;
            renderer.sharedMaterial.enableInstancing = true;
            spawned.Add(road);
        }

        private void SpawnBuilding(Vector3 position, Vector2 footprint, float height, Material mat)
        {
            var root = new GameObject("Building");
            root.transform.position = position;
            root.transform.rotation = Quaternion.Euler(0f, RandomRange(0f, 360f), 0f);

            var lodGroup = root.AddComponent<LODGroup>();
            var lods = new List<LOD>
            {
                CreateLod(root.transform, footprint, height, mat, 0.6f, "LOD0"),
                CreateLod(root.transform, footprint * 0.7f, height * 0.6f, mat, 0.3f, "LOD1"),
                CreateLod(root.transform, footprint * 0.4f, height * 0.3f, mat, 0.05f, "LOD2")
            };
            lodGroup.SetLODs(lods.ToArray());
            lodGroup.RecalculateBounds();

            var collider = root.AddComponent<BoxCollider>();
            collider.size = new Vector3(footprint.x, height, footprint.y);
            collider.center = new Vector3(0f, height * 0.5f, 0f);

            spawned.Add(root);
        }

        private LOD CreateLod(Transform parent, Vector2 footprint, float height, Material mat, float screenRelativeHeight, string name)
        {
            var lodObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lodObject.name = name;
            lodObject.transform.SetParent(parent, false);
            lodObject.transform.localPosition = new Vector3(0f, height * 0.5f, 0f);
            lodObject.transform.localScale = new Vector3(footprint.x, height, footprint.y);

            var renderer = lodObject.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = mat;
            renderer.sharedMaterial.enableInstancing = true;

            return new LOD(screenRelativeHeight, new[] { renderer });
        }

        private Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { color = color };
            material.enableInstancing = true;
            return material;
        }

        private float RandomRange(float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }
    }
}
