using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCopy : MonoBehaviour
{
    //[SerializeField] List<GameObject> TreeModels = new List<GameObject>();
    //[SerializeField] Transform TreesParent;

    //[SerializeField] float rate=1;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    float t = Time.realtimeSinceStartup;
    //    createTreeAreas();
    //    print(Time.realtimeSinceStartup - t);
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    //void createTreeAreas()
    //{
    //    int countArea = Random.Range(6,14);
    //    print(countArea);
    //    while(countArea-->0)
    //    {
    //        int width = Random.Range(10, 40);
    //        int height = Random.Range(15, 45);
    //        float sizeX = GetComponent<MeshRenderer>().bounds.size.x;
    //        float sizeY = GetComponent<MeshRenderer>().bounds.size.z;
    //        Vector3 center = GetComponent<MeshRenderer>().bounds.center;
    //        float PositionX=center.x;
    //        while(Mathf.Abs(PositionX - center.x)<20)
    //            PositionX = Random.Range((int)(center.x - (sizeX / 2) + (width / 2)) + 2, (int)(center.x + (sizeX / 2) - (width / 2)) - 2);
    //        float PositionY = center.z;
    //        while (Mathf.Abs(PositionY - center.z) <20)
    //            PositionY = Random.Range((int)(center.z - (sizeY / 2) + (height / 2)) + 2, (int)(center.z + (sizeY / 2) - (height / 2))-2);
    //        SpawnTrees(new Vector3(PositionX, 0, PositionY), width, height);
    //    }
    //}
    //void SpawnTrees(Vector3 center, int width, int height)
    //{
    //    int random = Random.Range(-7, 8);
    //    int randomP = Random.Range(2, 10);
    //    float minZ = center.z - (height / 2);
    //    float maxZ = center.z + (height / 2);
    //    float minX = center.x - (width / 2);
    //    float maxX = center.x + (width / 2);
    //    for (float z = minZ + random; z< maxZ - random; z += 3)
    //    {
    //        random = Random.Range(-5, 6);
    //        for (float x = minX + random; x < maxX - random; x += randomP)
    //        {
    //            randomP = Random.Range(2, 8);
    //            int RandomIndex = Random.Range(0, TreeModels.Count);
    //            Instantiate(TreeModels[RandomIndex], new Vector3(x, 0, z+randomP-3), Quaternion.identity, TreesParent).transform.localScale = new Vector3(randomP/5, randomP / 4, randomP / 5);
    //        }
    //    }
    //}
}
