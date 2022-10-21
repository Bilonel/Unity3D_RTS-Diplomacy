using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] public string CityName;
    [SerializeField] public Color color;

    public CityInventory inventory=new CityInventory();
    [SerializeField] public Dictionary<BuildType, int> Builds = new Dictionary<BuildType, int>();
    [SerializeField] public List<Item> itemPrefabs = new List<Item>();

    [SerializeField] private float wood = 0;
    [SerializeField] private float stone = 0;
    [SerializeField] private float population = 0;
    [SerializeField] private float food = 0;
    [SerializeField] private float gold = 0;
    public CityDesicion desicion;
    // Start is called before the first frame update
    void Start()
    {
        /// FOR TEST
        inventory.add("Wood", wood);
        inventory.add("Stone", stone);
        inventory.add("Food", food);
        inventory.add("Population", population);
        inventory.add("Gold", gold);
        ///
        Builds_Add(BuildType.FarmMill);
        Builds_Add(BuildType.Quarry);
        Builds_Add(BuildType.FarmMill);
        Builds_Add(BuildType.House);
        Builds_Add(BuildType.House);
        Builds_Add(BuildType.House);

        Refresh();
    }
    public void Refresh()
    {
        desicion = new CityDesicion(inventory.list, Builds);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public enum BuildType
    {
        FarmMill,
        LumberMill,
        Quarry,
        TradeHub,
        House
    }
    public void Builds_Add(BuildType KEY)
    {
        if (Builds.ContainsKey(KEY))   // IF KEY IS EXIST
            Builds[KEY]++;             // THEN INCREASE VALUE BY 1
        else
            Builds.TryAdd(KEY, 1);
    }
}
