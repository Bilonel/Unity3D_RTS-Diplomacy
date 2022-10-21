using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public static class PathNodes
{
    static int gridSize = 150;

    public static Node[] originalNodes;
    public static Node[] Nodes
    {
        get
        {
            if(originalNodes==null)
            {
                Grid grid = GameObject.FindObjectOfType<Grid>();
                originalNodes = new Node[gridSize*gridSize];
                for (int x = 0; x < gridSize; x++)
                    for (int y = 0; y < gridSize; y++)
                    {
                        int index = PositionToIndex(x, y);
                        bool walkable = !grid.grid[x, y].isWater;
                        originalNodes[index]= new Node(x, y, walkable);
                    }
            }
            return originalNodes;
        }
    }


    static int PositionToIndex(int x,int y)
    {
        return x + y * gridSize;
    }


}
public struct Node
{
    public int x, y;

    public int RestCost;
    public int WalkCost;
    public int TotalCost;

    public bool isWalkable;
    public int previousIndex;

    public Node(int x, int y, bool walkable = true)
    {
        this.x = x;
        this.y = y;
        RestCost = int.MaxValue;

        WalkCost = int.MaxValue;
        previousIndex = -1;
        isWalkable = walkable;
        TotalCost = int.MaxValue;
    }
    public void CalculateTotalCost(int WalkCost, int2 endPos)
    {
        this.WalkCost = WalkCost;
        this.RestCost = CalculateLowestCost(new int2(x, y), endPos);
        TotalCost = WalkCost + RestCost;
    }
    int CalculateLowestCost(int2 startPosition, int2 endPosition)
    {
        int diffX = math.abs(endPosition.x - startPosition.x);
        int diffY = math.abs(endPosition.y - startPosition.y);
        int remaining = math.abs(diffX - diffY);
        return 14 * math.min(diffY, diffX) + 10 * remaining;
    }
}

