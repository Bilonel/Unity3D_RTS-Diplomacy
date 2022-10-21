using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Villagers
{
    public static List<GameObject> UnemployedVillagers=new List<GameObject>();
    public static List<GameObject> EmployedVillagers = new List<GameObject>();
    private static List<GameObject> Flags=new List<GameObject>();
    public static void GiveJob(GameObject Workplace,int count,bool employee=true)
    {
        if (count == -1) count = UnemployedVillagers.Count;
        if (UnemployedVillagers.Count < count)
            return;
            //Debug.Log("NOT ENOUGH VILLAGER : ERROR");
            //throw new System.Exception("ERROR : Not Enough Villagers");

        if(Workplace.name.Contains("Flag"))
        {
            Flags.Add(Workplace);
            setCollecter();
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                if (UnemployedVillagers[0].GetComponent<villagerMovement>().setWorkplace(Workplace)) continue;
                if (Workplace.name.Contains("FarmMill"))
                    UnemployedVillagers[0].AddComponent<Farmer>();
                else if (Workplace.name.Contains("Builder"))
                    UnemployedVillagers[0].AddComponent<Builder>();
                else if (Workplace.name.Contains("Lumber"))
                    UnemployedVillagers[0].AddComponent<Lumber>();
                EmployedVillagers.Add(UnemployedVillagers[0]);
                UnemployedVillagers.RemoveAt(0);
            }
            GameObject.Find("UpperPanel").GetComponent<GUI>().Population = UnemployedVillagers.Count;
        }
    }
    public static void NewVillager(GameObject Villager, Vector3 housePosition)
    {
        Villager.GetComponent<villagerMovement>().housePosition = housePosition;
        UnemployedVillagers.Add(Villager);
        GameObject.Find("UpperPanel").GetComponent<GUI>().Population = UnemployedVillagers.Count;
        setCollecter();
    }
    static private void setCollecter()
    {
        int a = 0;
        for (int i = 0; i < UnemployedVillagers.Count; i++)
        {
            UnemployedVillagers[i].GetComponent<villagerMovement>().setWorkplace(Flags[a]);
            if (UnemployedVillagers[i].GetComponent<Collecter>() == null)
                UnemployedVillagers[i].AddComponent<Collecter>();
            else UnemployedVillagers[i].GetComponent<Collecter>().reset();
            a++;
            if (a >= Flags.Count) a = 0;
        }
    }
}
