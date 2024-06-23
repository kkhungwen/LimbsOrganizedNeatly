using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomGrid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector2 originPosition;
    private TGridObject[,] gridArray;


    public CustomGrid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject();
            }
        }

        /*
        bool showDebug = true;
        if (showDebug)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
        }
        */
    }


    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    // Get specific world position of a grid
    public Vector2 GetWorldPosition(Vector2 worldPosition)
    {
        Vector2Int gridPosition = GetGridPosition(worldPosition);
        Vector2 gridWorldPosition = GetWorldPosition(gridPosition);

        return gridWorldPosition;
    }

    public Vector2 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector2(gridPosition.x, gridPosition.y) * cellSize + originPosition + new Vector2(cellSize, cellSize) / 2;
    }

    public Vector2Int GetGridPosition(Vector2 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);

        return new Vector2Int(x, y);
    }

    public void SetValue(Vector2Int gridPosition, TGridObject value)
    {
        if (gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < width && gridPosition.y < height)
        {
            gridArray[gridPosition.x, gridPosition.y] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        Vector2Int gridPosition = GetGridPosition(worldPosition);
        SetValue(gridPosition, value);
    }

    public bool TryGetValue(Vector2Int gridPosition, out TGridObject gridObject)
    {
        if (gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < width && gridPosition.y < height)
        {
            gridObject = gridArray[gridPosition.x, gridPosition.y];
            return true;
        }
        else
        {
            gridObject = default;
            return false;
        }
    }

    public bool TryGetValue(Vector3 worldPosition, out TGridObject gridObject)
    {
        Vector2Int gridPosition = GetGridPosition(worldPosition);

        return TryGetValue(gridPosition, out gridObject);
    }

    public IEnumerable<TGridObject> GetValues()
    {
        foreach (TGridObject value in gridArray)
        {
            yield return value;
        }
    }
}
