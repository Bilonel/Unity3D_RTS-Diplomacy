using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] int IncrementPopulation = 3;
    [SerializeField] GameObject Villager;
    [SerializeField] Transform villagersParent;

    // Start is called before the first frame update
    void Start()
    {
        CustomStart();
    }
    private void Update()
    {
        CustomStart();
    }
    void CustomStart()
    {
        if (!GetComponent<Building>().isBuildComplete) return;
        if (!Cursor.visible) return;

        villagersParent = GameObject.Find("Villagers").transform;
        SpawnVillagers();
        GetComponent<House>().enabled = false;
    }
    void SpawnVillagers()
    {
        for (int i = 0; i < IncrementPopulation; i++)
            try{ Villagers.NewVillager(Instantiate(Villager, transform.position + new Vector3(1,0,1) ,Quaternion.identity, villagersParent), transform.position); }
            catch (System.Exception e)
            { print(e.Message); }
    }

}
