using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    [SerializeField]
    private float gridWidth = 1f;
    [SerializeField]
    private float gridHeight = 1f;

    private List<GameObject> blockList;
    private int[,] gridMap;
    private int width = 0, height = 0;

    private GameObject[,] blockMap;

    public int Width => width;
    public int Height => height;
    public int[,] GridMap => gridMap;
    public float GridWidth => gridWidth;
    public float GridHeight => gridHeight;

    private void Start()
    {

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

        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
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
}
