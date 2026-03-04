using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerPlacer : MonoBehaviour
{
    public GameObject monsterSpawnerPrefab;
    public int spawnCount = 5;
    public float minDistance = 5f;

    public void Place(MapData map, float tileSize)
    {
        List<Vector3> placedPositions = new List<Vector3>();
        List<Vector3> floorPositions = CollectFloorPositions(map, tileSize);

        int tryCount = 0;

        while (placedPositions.Count < spawnCount && tryCount < 500)
        {
            tryCount++;

            Vector3 candidate = floorPositions[Random.Range(0, floorPositions.Count)];

            if (SpawnUtility.IsFarEnough(candidate, placedPositions, minDistance))
            {
                placedPositions.Add(candidate);
                Instantiate(monsterSpawnerPrefab, candidate, Quaternion.identity);
            }
        }
    }

    private List<Vector3> CollectFloorPositions(MapData map, float tileSize)
    {
        List<Vector3> result = new List<Vector3>();

        float offsetX = map.Width * tileSize * 0.5f;
        float offsetZ = map.Height * tileSize * 0.5f;

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map.Get(x, y) != TileType.Wall)
                {
                    Vector3 pos = new Vector3(
                        x * tileSize - offsetX,
                        0,
                        y * tileSize - offsetZ);

                    result.Add(pos);
                }
            }
        }

        return result;
    }
}