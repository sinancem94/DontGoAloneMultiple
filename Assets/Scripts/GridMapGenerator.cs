using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class GridMapGenerator 
{
    public struct GridData
    {
        public Vector2 gridPos;
        public bool occupied;
    }
    
    public GridData[,] gridMatrix; 
    
    private readonly int _mapSize;
    private Transform _sampleGrid;
    private Vector3 initialGridPosition;
    
    public GridMapGenerator(int mapSize,Vector3 initialGridPosition)
    {
        _mapSize = mapSize;
        
        gridMatrix = new GridData[_mapSize,_mapSize];
        _sampleGrid = Resources.Load<Transform>("Prefabs/Rooms/grid");
        this.initialGridPosition = initialGridPosition;
        
        GenerateGridMap();
    }

    public void GenerateGridMap()
    {
        GameObject GridParent = new GameObject("GridMap");
        
        float gridSize = _sampleGrid.localScale.z + 1f;
        
        int key = 0;
        
        for (int col = 0; col < _mapSize; col++)
        {
            for (int row = 0; row < _mapSize ; row++)
            {
                Vector3 pos = new Vector3(initialGridPosition.x + (col * gridSize),0f,initialGridPosition.z + (row * gridSize));
                Transform grid = UnityEngine.Object.Instantiate(_sampleGrid, pos, Quaternion.identity, GridParent.transform);
                grid.gameObject.SetActive(false);
                
                GridData gridData = new GridData();
                gridData.gridPos = new Vector2(pos.x, pos.z);
                gridData.occupied = false;

                gridMatrix[col, row] = gridData;
                
                key++;
            }
        }
    }
}
