using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Lumber : MonoBehaviour
{
    [SerializeField] float SizeOfArea=30;
    [SerializeField] float Damage=10f;

    List<GameObject> Trees = new List<GameObject>();
    List<GameObject> CuttedTrees = new List<GameObject>();
    List<GameObject> Saplings = new List<GameObject>();
    bool[,] SaplingMap;

    villagerMovement villager;
    Transform SelectedTree;
    int step = 0;
    bool isArrived = false;

    Vector3 targetSaplingPosition=Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        SelectedTree = null;
        villager = GetComponent<villagerMovement>();
        SizeOfArea = 30;
        Collider[] colliders = Physics.OverlapBox(villager.workplace.transform.position, SizeOfArea / 2 * Vector3.one, Quaternion.identity, LayerMask.GetMask("Tree"));
        foreach (var item in colliders)
            Trees.Add(item.gameObject);

        SaplingMap = new bool[((int)SizeOfArea), ((int)SizeOfArea)];
        
    }

    // Update is called once per frame
    public void Work()
    {
        switch(step)
        {
            case 0: ReturnWorkplace();break;
            case 1: GoTree();break;
            case 2: CutTree();break;
            case 4: findValidPlantingPosition();break;
            case 5: GoPlanting();break;
            default: break;
        }
    }
    void SelectTree()
    {
        if (Trees.Count > 0)
        {
            SelectedTree = Trees[UnityEngine.Random.Range(0, Trees.Count - 1)].transform;
            Trees.Remove(SelectedTree.gameObject);
        }
        else
        {
            for (int y = 0; y < ((int)SizeOfArea); y++)
            {
                for (int i = 0; i < ((int)SizeOfArea); i++)
                {
                    SaplingMap[y, i] = false;
                }
            }
            step = 4;
        }
    }
    void GoTree()
    {
        if (SelectedTree == null)
            SelectTree();
        else
        {
            if (SelectedTree == null) return;
            if (isArrived)
            {
                isArrived=villager.arrived = false;
                step = 2;
                return;
            }
            villager.MoveTo(SelectedTree.position);
            isArrived = villager.arrived;
        }
    }
    void CutTree()
    {
        if (SelectedTree == null) return;
        if (!SelectedTree.GetComponent<resource>().Damage(Damage))
        {
            SelectedTree = null;
            step = 0;
        }
    }
    void ReturnWorkplace()
    {
        if (villager.arrived)
        {
            isArrived = villager.arrived = false;
            if (Trees.Count < 1) step = 4;
            else step = 1;
        }
        villager.MoveTo(villager.workplace.transform.position);
    }
    void findValidPlantingPosition()
    {
        for (int y = 0; y < ((int)SizeOfArea); y++)
        {
            for (int x = 0; x < ((int)SizeOfArea); x++)
            {
                if (!SaplingMap[x, y])
                {
                    targetSaplingPosition = IndexToPosition(x, y);
                    step = 5;
                    return;
                }
            }
        }
        Collider[] colliders = Physics.OverlapBox(villager.workplace.transform.position, SizeOfArea / 2 * Vector3.one, Quaternion.identity, LayerMask.GetMask("Tree"));
        foreach (var item in colliders)
            Trees.Add(item.gameObject);
        step = 0;
    }
    void GoPlanting()
    {
        villager.MoveTo(targetSaplingPosition);
        if(villager.arrived)
        {
            villager.arrived = false;
            PlantSapling();
            step = 0;
        }

    }
    void PlantSapling()
    {
        Instantiate(villager.workplace.GetComponent<LumberMill>().saplingPrefab, villager.workplace.GetComponent<LumberMill>().SaplingsParent);
    }
    int2 positionToIndex(Vector3 pos)
    {
        Vector3 wpPos = villager.workplace.transform.position;
        float xMin = wpPos.x - SizeOfArea / 2;
        float yMin = wpPos.z - SizeOfArea / 2;
        float xMax = xMin + SizeOfArea;
        float yMax = yMin + SizeOfArea;

        return new int2(((int)(pos.x - xMin)),((int)(pos.z - yMin)));
    }
    Vector3 IndexToPosition(int x,int y)
    {
        Vector3 wpPos = villager.workplace.transform.position;
        float xMin = wpPos.x - SizeOfArea / 2;
        float yMin = wpPos.z - SizeOfArea / 2;

        return new Vector3(x,0,y) + new Vector3(xMin, 0, yMin);
    }
}
