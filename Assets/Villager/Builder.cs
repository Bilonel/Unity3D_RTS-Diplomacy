using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    float BuildCompletionPerBuilder = 1f;
    villagerMovement villager;
    GameObject currentBuild;
    int step = 2;
    private void Start()
    {
        villager = GetComponent<villagerMovement>();
    }
    public void Work()
    {
        switch(step)
        {
            case 0: GoBuilding();
                break;
            case 1: Build();
                break;
            case 2: ReturnHub();
                break;

            default:break;
        }    
    }
    void GoBuilding()
    {
        if (currentBuild == null) return;
        if(!villager.arrived)
            villager.MoveTo(currentBuild.transform.position);
        else
        {
            villager.arrived = false;
            step++;
        }
    }
    Transform targetTree; bool cleaned = false;
    void Build()
    {
        if (!cleaned) Clean();
        else
        {
            Building build = currentBuild.GetComponent<Building>();
            if (build == null || build.isBuildComplete) step++;
            else build.Build(BuildCompletionPerBuilder * Time.deltaTime);
        }
    }
    void Clean()
    {
        if(targetTree==null)
        {
            float size = currentBuild.GetComponent<Building>().BuildingSize.x;
            var trees = Physics.OverlapBox(currentBuild.transform.position,  size/ 2 * Vector3.one, Quaternion.identity, LayerMask.GetMask("Tree"));
            if (trees.Length < 1)
            {
                cleaned=true;
                return;
            }
            targetTree = trees[Random.Range(0, trees.Length)].transform;
        }
        else if (villager.arrived)
        {
            targetTree.GetComponent<Animator>().SetTrigger("Cut");
            targetTree = null;
            villager.arrived = false;
        }
        else
        {
            villager.MoveTo(targetTree.position);
        }
    }
    void ReturnHub()
    {
        if(villager.arrived)
        {
            if (BuildList.builds.Count == 0) return;
            currentBuild = BuildList.builds[0];
            step = 0;
            cleaned = false;
            villager.onPath = false;
            villager.arrived = false;
            return;
        }
        villager.MoveTo(villager.workplace.transform.position);
    }

    bool isArrive(Vector3 pos,float range=1f)
    {
        return (pos - transform.position).magnitude < range;
    }

}
