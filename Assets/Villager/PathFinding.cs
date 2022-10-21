using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using System;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    int gridSize=150;
    int i = 0;

    private void Start()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        gridSize = grid.size;
        
        
    }
    private void Update()
    {


        //if (++i > 125) return;

        //float start = Time.realtimeSinceStartup;

        //FindPath(new int2(20, 20), new int2(40, 40));
        //Debug.Log("Time : " + ((Time.realtimeSinceStartup - start) * 1000f));
    }
    public List<int2> getPath(Vector3 startPos,Vector3 endPos)
    {
        List<int2> path = new List<int2>();
        FindPath(new int2(((int)startPos.x),((int)startPos.z)),new int2(((int)endPos.x),((int)endPos.z)),path);
        return path;
    }
    void FindPath(int2 startPosition,int2 endPosition,List<int2> result)
    {
        NativeArray<Node> NodeArray = new NativeArray<Node>(PathNodes.Nodes, Allocator.Temp);
        //
        // SET END NODE
        int EndIndex = PositionToIndex(endPosition.x, endPosition.y);
        //
        // SET START NODE
        int StartIndex = PositionToIndex(startPosition.x, startPosition.y);
        Node startNode = NodeArray[StartIndex];
        startNode.CalculateTotalCost(0,endPosition);
        NodeArray[StartIndex] = startNode;
        //
        // CREATE LISTS

        NativeArray<int2> NeighboutOffsets = new NativeArray<int2>(new int2[]
        {
            new int2(-1,0),
            new int2(+1,0),
            new int2(0,-1),
            new int2(0,+1),
            new int2(-1,-1),
            new int2(-1,+1),
            new int2(+1,-1),
            new int2(+1,+1),
        }, Allocator.Temp);

        NativeList<int> ActiveIndexes = new NativeList<int>(Allocator.Temp);
        NativeList<int> PassiveIndexes = new NativeList<int>(Allocator.Temp);

        ActiveIndexes.Add(StartIndex);

        while(ActiveIndexes.Length>0)
        {
            int currentIndex = GetShortestNodeInActives(ActiveIndexes, NodeArray);
            Node currentNode = NodeArray[currentIndex];
            if(currentIndex==EndIndex)
            {
                break;
            }
            for (int i = 0; i < ActiveIndexes.Length; i++)
                if (currentIndex == ActiveIndexes[i])
                {
                    ActiveIndexes.RemoveAtSwapBack(i);
                    break;
                }
            PassiveIndexes.Add(currentIndex);

            for (int i = 0; i < 8; i++)
            {
                int2 offset = NeighboutOffsets[i];

                int2 Nposition = new int2(currentNode.x + offset.x, currentNode.y + offset.y);
                if (!isInsideGrid(Nposition)) continue;
                
                int Nindex = PositionToIndex(Nposition.x, Nposition.y);
                if (PassiveIndexes.Contains(Nindex)) continue;
                
                Node Nnode = NodeArray[Nindex];
                if (!Nnode.isWalkable) continue;

                int2 CurrentNodePosition = new int2(currentNode.x, currentNode.y);
                int TentativeCost = currentNode.WalkCost + CalculateLowestCost(CurrentNodePosition, Nposition);
                if(TentativeCost<Nnode.WalkCost)
                {
                    Nnode.CalculateTotalCost(TentativeCost,endPosition);
                    Nnode.previousIndex = currentIndex;
                    NodeArray[Nindex] = Nnode;
                    if (!ActiveIndexes.Contains(Nindex))
                        ActiveIndexes.Add(Nindex);
                }

            }
        }
        Node EndNode = NodeArray[EndIndex];
        if(EndNode.previousIndex==-1)
        {
            // Could Not A Valid Path
        }
        else
        {
            NativeList<int2> path = GetPath(NodeArray,EndNode);
            for(int i=0;i<path.Length;i++)
            {
                result.Add(path[i]);
            }
            path.Dispose();
        }


        NeighboutOffsets.Dispose();
        NodeArray.Dispose();
        PassiveIndexes.Dispose();
        ActiveIndexes.Dispose();
    }
    NativeList<int2> GetPath(NativeArray<Node> nodes , Node endNode)
    {
        if (endNode.previousIndex == -1) return new NativeList<int2>(Allocator.Temp);
        NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
        path.Add(new int2(endNode.x, endNode.y));
        Node currentNode = endNode;
        while(currentNode.previousIndex!=-1)
        {
            Node PreNode = nodes[currentNode.previousIndex];
            path.Add(new int2(PreNode.x,PreNode.y));
            currentNode = PreNode;
        }

        return path;
    }

    bool isInsideGrid(int2 pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }
    
    int GetShortestNodeInActives(NativeList<int> CurrentNodes , NativeArray<Node> WholeNodes)
    {
        int ShortestIndex = CurrentNodes[0];
        for (int index = 0; index < CurrentNodes.Length; index++)
        {
            int node_index = CurrentNodes[index];
            if (WholeNodes[node_index].TotalCost < WholeNodes[ShortestIndex].TotalCost)
            {
                ShortestIndex = node_index;
            }
        }
        return ShortestIndex;
    }
    int PositionToIndex(int x,int y)
    {
        return y*gridSize+x;
    }
    int CalculateLowestCost(int2 startPosition,int2 endPosition)
    {
        int diffX = math.abs(endPosition.x - startPosition.x);
        int diffY = math.abs(endPosition.y - startPosition.y);
        int remaining = math.abs(diffX - diffY);
        return 14 * math.min(diffY, diffX) + 10 * remaining;
    }
}
