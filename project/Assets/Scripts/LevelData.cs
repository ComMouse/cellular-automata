using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Boo.Lang.Runtime;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData instance { get; private set;}

    public GameObject[] blockPrefabs;
    [SerializeField]
    private float gridWidth = 1f;
    [SerializeField]
    private float gridHeight = 1f;

    private List<GameObject> blockList;
    private int[,] gridMap;
    private int width = 0, height = 0;

    private GameObject[,] blockMap;

    private List<SpawnPoint> playerSpawnPoints;
    private List<SpawnPoint> lootSpawnPoints;

    public int Width => width;

    public int Height => height;
    public int[,] GridMap => gridMap;
    public float GridWidth => gridWidth;
    public float GridHeight => gridHeight;

    public bool isGenerated;

    private void Awake()
    {
        instance = this;
        isGenerated = false;
    }

    private void Update()
    {

    }

    public void SetGridMap(int[,] gridMap)
    {
        this.gridMap = gridMap;
        width = gridMap.GetLength(1);
        height = gridMap.GetLength(0);
    }

    public void Generate()
    {
        blockList = new List<GameObject>();
        blockMap = new GameObject[height, width];
        playerSpawnPoints = new List<SpawnPoint>();
        lootSpawnPoints = new List<SpawnPoint>();

        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int grid = gridMap[i, j];
                if (grid >= (int)GridType.PlayerSpawn1 &&
                    grid <= (int)GridType.PlayerSpawn4)
                {
                    playerSpawnPoints.Add(
                        new SpawnPoint(
                            grid - (int)GridType.PlayerSpawn1, 
                            new LevelCoord(j, i)));
                }

                if (grid >= (int)GridType.LootSpawn1 &&
                    grid <= (int)GridType.LootSpawn8)
                {
                    lootSpawnPoints.Add(
                        new SpawnPoint(
                            grid - (int)GridType.LootSpawn1,
                            new LevelCoord(j, i)));
                }

                var prefab = GetPrefab(gridMap[i, j]);
                if (prefab == null)
                    continue;

                var go = Instantiate(prefab);
                go.name = $"Block ({j}, {i})";
                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(j * gridWidth, i * gridHeight, 0f);
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
            }
        }
        isGenerated = true;
    }

    private GameObject GetPrefab(int type)
    {
        return GetPrefab((GridType) type);
    }

    private GameObject GetPrefab(GridType type)
    {
        if (type == GridType.Empty || type == GridType.None)
            return null;

        if ((int) type >= blockPrefabs.Length)
            return null;

        return blockPrefabs[(int) type];
    }

    public Vector3 Coord2LocalPos(LevelCoord coord)
    {
        return Coord2LocalPos(coord.x, coord.y);
    }

    public Vector3 Coord2LocalPos(int x, int y)
    {
        return new Vector3(x * gridWidth, y * gridHeight, 0f);
    }

    public Vector3 Coord2WorldPos(LevelCoord coord)
    {
        return Coord2WorldPos(coord.x, coord.y);
    }

    public Vector3 Coord2WorldPos(int x, int y)
    {
        return transform.TransformPoint(Coord2LocalPos(x, y));
    }

    public LevelCoord LocalPos2Coord(Vector3 pos)
    {
        return new LevelCoord(Mathf.RoundToInt(pos.x / gridWidth), Mathf.RoundToInt(pos.y / gridHeight));
    }

    public LevelCoord WorldPos2Coord(Vector3 worldPos)
    {
        return LocalPos2Coord(transform.InverseTransformPoint(worldPos));
    }

    public List<SpawnPoint> GetAllPlayerSpawnPoints()
    {
        return playerSpawnPoints;
    }

    public List<LevelCoord> GetPlayerSpawnPoint(int id)
    {
        return playerSpawnPoints
            .FindAll(point => point.typeId == id)
            .Select(p => p.coord)
            .ToList();
    }

    public List<SpawnPoint> GetAllLootSpawnPoints()
    {
        return lootSpawnPoints;
    }

    public List<LevelCoord> GetLootSpawnPoint(int id)
    {
        return lootSpawnPoints
            .FindAll(point => point.typeId == id)
            .Select(p => p.coord)
            .ToList();
    }

    public bool IsEmpty(int grid)
    {
        return grid == (int)GridType.None || grid == (int)GridType.Empty;
    }

    public bool IsOccupiedByPlayer(int grid)
    {
        return grid < -1;
    }

    public bool IsWall(int grid)
    {
        return grid == (int)GridType.Wall;
    }
}

public struct SpawnPoint
{
    public int typeId;
    public LevelCoord coord;

    public SpawnPoint(int typeId, LevelCoord coord)
    {
        this.typeId = typeId;
        this.coord = coord;
    }
}

[System.Serializable]
public struct LevelCoord
{
    public int x, y;

    public LevelCoord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator==(LevelCoord coordA, LevelCoord coordB)
    {
        return coordA.x == coordB.x && coordA.y == coordB.y;
    }

    public static bool operator!=(LevelCoord coordA, LevelCoord coordB)
    {
        return !(coordA == coordB);
    }

    public static LevelCoord operator +(LevelCoord coordA, LevelCoord coordB)
    {
        return new LevelCoord(coordA.x + coordB.x, coordA.y + coordB.y);
    }

    public static LevelCoord operator -(LevelCoord coordA, LevelCoord coordB)
    {
        return new LevelCoord(coordA.x - coordB.x, coordA.y - coordB.y);
    }
}