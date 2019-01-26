﻿using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<int, LevelCoord> playerSpawnPoints;
    private Dictionary<int, LevelCoord> lootSpawnPoints;

    public int Width => width;

    public int Height => height;
    public int[,] GridMap => gridMap;
    public int[,] OriginMap;
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
        OriginMap = gridMap;
        width = gridMap.GetLength(1);
        height = gridMap.GetLength(0);
    }

    public void Generate()
    {
        blockList = new List<GameObject>();
        blockMap = new GameObject[height, width];
        playerSpawnPoints = new Dictionary<int, LevelCoord>();
        lootSpawnPoints = new Dictionary<int, LevelCoord>();

        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                int grid = gridMap[i, j];
                if (grid >= (int)GridType.PlayerSpawn1 &&
                    grid <= (int)GridType.PlayerSpawn4)
                {
                    playerSpawnPoints[(grid - (int) GridType.PlayerSpawn1) + 1] =
                        new LevelCoord(j, i);
                }

                if (grid >= (int)GridType.LootSpawn1 &&
                    grid <= (int)GridType.LootSpawn8)
                {
                    lootSpawnPoints[(grid - (int)GridType.LootSpawn1) + 1] =
                        new LevelCoord(j, i);
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
        LootItemManager.instance.StartGame();
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

    public LevelCoord GetPlayerSpawnPoint(int id)
    {
        if (!playerSpawnPoints.ContainsKey(id))
        {
            throw new RuntimeException($"Player Spawn point of id {id} does not exist");
        }

        return playerSpawnPoints[id];
    }

    public LevelCoord GetLootSpawnPoint(int id)
    {
        if (!lootSpawnPoints.ContainsKey(id))
        {
            throw new RuntimeException($"Loot Spawn point of id {id} does not exist");
        }

        return lootSpawnPoints[id];
    }

    public bool IsEmpty(int grid)
    {
        return grid == (int)GridType.None || grid == (int)GridType.Empty || IsLootItem(grid);
    }

    public bool IsOccupiedByPlayer(int grid)
    {
        return grid < -1;
    }

    public bool IsWall(int grid)
    {
        return grid == (int)GridType.Wall;
    }

    public bool IsLootItem(int grid)
    {
        return grid >= (int)GridType.LootSpawn1 && grid <= (int)GridType.LootSpawn8;
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