using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Size")]
    public int width = 50;
    public int height = 50;

    [Header("Renderer")]
    public MapRenderer renderer;

    [Header("Placer")]
    public MonsterSpawnerPlacer monsterPlacer;
    public ItemSpawnerPlacer itemPlacer;
    public BossSpawnPlacer bossPlacer;

    private IMapGenerationStrategy strategy;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        MapData map = new MapData(width, height);

        // 전략 선택
        //strategy = new HybridBSPStrategy(minRoomSize: 6, maxDepth: 4);
        strategy = new CellularAutomataStrategy(45, 2);

        strategy.Generate(map);

        renderer.Render(map);
        monsterPlacer.Place(map, renderer.tileSize);
        itemPlacer.Place(map, renderer.tileSize);
        bossPlacer.Place(map, renderer.tileSize);
    }
}