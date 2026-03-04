using UnityEngine;

public class CellularAutomataStrategy : IMapGenerationStrategy
{
    private int fillPercent;
    private int smoothIterations;

    public CellularAutomataStrategy(int fillPercent = 45, int smoothIterations = 2)
    {
        this.fillPercent = fillPercent;
        this.smoothIterations = smoothIterations;
    }

    public void Generate(MapData map)
    {
        RandomFill(map);

        for (int i = 0; i < smoothIterations; i++)
        {
            Smooth(map);
        }
    }

    private void RandomFill(MapData map)
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (IsBorder(map, x, y))
                {
                    map.Set(x, y, TileType.Wall);
                }
                else
                {
                    map.Set(x, y,
                        Random.Range(0, 100) < fillPercent
                        ? TileType.Wall
                        : GetRandomFloorType());
                }
            }
        }
    }
    private TileType GetRandomFloorType()
    {
        int r = Random.Range(0, 3);

        switch (r)
        {
            case 0: return TileType.FloorA;
            case 1: return TileType.FloorB;
            default: return TileType.FloorC;
        }
    }

    private void Smooth(MapData map)
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                int wallCount = CountWallNeighbours(map, x, y);

                if (wallCount >= 5)
                    map.Set(x, y, TileType.Wall);
                else if (wallCount < 5)
                    map.Set(x, y, GetRandomFloorType());
            }
        }
    }

    private int CountWallNeighbours(MapData map, int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                if (map.Get(x + dx, y + dy) == TileType.Wall)
                    count++;
            }
        }

        return count;
    }

    private bool IsBorder(MapData map, int x, int y)
    {
        return x == 0 || y == 0 || x == map.Width - 1 || y == map.Height - 1;
    }
}