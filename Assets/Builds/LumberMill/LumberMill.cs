using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LumberMill : MonoBehaviour
{
    [SerializeField] public float SizeOfArea;
    [SerializeField] public GameObject saplingPrefab;
    public Transform SaplingsParent;
    List<GameObject> Trees = new List<GameObject>();
    List<GameObject> CuttedTrees = new List<GameObject>();
    List<GameObject> Saplings = new List<GameObject>();
    bool[,] SaplingMap;
    bool[,] OriginalSaplingMap=null;
    int sizeSaplingMap;
    Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        if(Cursor.visible)
            Villagers.GiveJob(this.gameObject, 2);
        grid = GameObject.FindObjectOfType<Grid>();
        FindTrees();
        sizeSaplingMap = ((int)SizeOfArea / 5);
        SaplingMap = new bool[sizeSaplingMap,sizeSaplingMap];
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(SizeOfArea,0.1f,SizeOfArea));
    }
    public void FindTrees()
    {
        Trees.Clear();
        Collider[] colliders = Physics.OverlapBox(transform.position, SizeOfArea / 2 * Vector3.one, Quaternion.identity, LayerMask.GetMask("Tree"));
        foreach (var item in colliders)
            Trees.Add(item.gameObject);
    }
    public GameObject getTree()
    {
        if (Trees.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, Trees.Count - 1);
            GameObject tree = Trees[random];
            Trees.RemoveAt(random);
            return tree;
        }
        else return null;
    }
    public void initSaplingMap()
    {
        float xMin = transform.position.x - SizeOfArea / 2;
        float yMin = transform.position.z - SizeOfArea / 2;
        SaplingMap = new bool[sizeSaplingMap, sizeSaplingMap];
        for (int x = 0; x < sizeSaplingMap; x++)
        {
            for (int y = 0; y < sizeSaplingMap; y++)
            {
                int2 pos = new int2(((int)xMin) + 5 * x, ((int)yMin) + 5 * y);
                bool isWater = false;
                if (pos.x > 0 && pos.y > 0 && pos.x < grid.size && pos.y < grid.size)
                    isWater = grid.grid[pos.x, pos.y].isWater;
                bool isOverLapping = Physics.CheckSphere(new Vector3(pos.x, 0, pos.y), 3,LayerMask.GetMask("Build"));
                SaplingMap[x, y] = isWater || isOverLapping;
                if (isWater || isOverLapping) print("*****************************************************************");
            }
        }
    }
    public void setSaplingMap(int2 index,bool flag)
    {
        SaplingMap[index.x,index.y] = flag;
    }
    public int2 getFirstValidSaplingArea()
    {
        for (int x = 0; x < sizeSaplingMap; x++)
        {
            for (int y = 0; y < ((int)sizeSaplingMap); y++)
            {
                if (!SaplingMap[x, y])
                {
                    SaplingMap[x, y] = true;
                    return new int2(x,y);
                }
            }
        }
        return new int2(-1,-1);
    }
    public bool areTreesCutted() { return Trees.Count == 0; }
}
