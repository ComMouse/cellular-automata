using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader
{
    private int[,] gridMap;

    public LevelLoader() {
        //
    }

    public void Load(string path = "")
    {
        var levelTxt = Resources.Load<TextAsset>(path);
        if (levelTxt == null)
        {
            Debug.LogError($"Level [{path}] does not exist!");
            return;
        }

        var levelStr = levelTxt.text;

        var levelRows = levelStr.Split('\n');
        var levelHeight = levelRows.Length - 1;

        for (var i = 0; i < levelHeight; ++i)
        {
            var rowStr = levelRows[i];
            var rowGrids = rowStr.Split(',');

            var rowWidth = rowGrids.Length;

            if (gridMap == null)
            {
                gridMap = new int[levelHeight + 2,rowWidth + 2];
                InitGridMap(rowWidth + 2, levelHeight + 2);
                SetGridBorder();
            }

            for (var j = 0; j < rowWidth; ++j)
            {
                try
                {
                    int grid = Int32.Parse(rowGrids[j]);
                    gridMap[rowWidth - i, levelHeight - j] = grid;
                }
                catch
                {
                    Debug.LogError($"Error reading value in Grid ({j}, {i})");
                }
            }
        }
    }

    public int[,] GetMap()
    {
        return gridMap;
    }

    private void InitGridMap(int w, int h)
    {
        for (int i = 0; i < h; ++i)
        {
            for (int j = 0; j < w; ++j)
            {
                gridMap[i, j] = (int)GridType.None;
            }
        }
    }

    private void SetGridBorder()
    {
        int w = gridMap.GetLength(1);
        int h = gridMap.GetLength(0);

        for (int i = 0; i < w; ++i)
        {
            gridMap[0, i] = (int)GridType.Wall;
            gridMap[h - 1, i] = (int)GridType.Wall;
        }

        for (int i = 0; i < h; ++i)
        {
            gridMap[i, 0] = (int)GridType.Wall;
            gridMap[i, w - 1] = (int)GridType.Wall;
        }
    }
}
