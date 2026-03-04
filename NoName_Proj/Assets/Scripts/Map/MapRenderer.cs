using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject floorAPrefab;
    public GameObject floorBPrefab;
    public GameObject floorCPrefab;
    public GameObject wallPrefab;

    [Header("Settings")]
    public float tileSize = 1f;

    private Transform container;

    public void Render(MapData map)
    {
        Clear();

        container = new GameObject("MapContainer").transform;
        container.parent = transform;
        float offsetX = map.Width * tileSize * 0.5f;
        float offsetZ = map.Height * tileSize * 0.5f;
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                GameObject prefab = null;

                switch (map.Get(x, y))
                {
                    case TileType.FloorA:
                        prefab = floorAPrefab;
                        break;
                    case TileType.FloorB:
                        prefab = floorBPrefab;
                        break;
                    case TileType.FloorC:                            
                        prefab = floorCPrefab;
                        break;
                    case TileType.Wall:
                        prefab = wallPrefab;
                        break;
                }

                if (prefab == null) continue;

                Vector3 position = new Vector3(
                    x * tileSize - offsetX,
                    0,
                    y * tileSize - offsetZ);

                Instantiate(prefab, position, Quaternion.identity, container);
            }
        }
    }

    private void Clear()
    {
        if (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}