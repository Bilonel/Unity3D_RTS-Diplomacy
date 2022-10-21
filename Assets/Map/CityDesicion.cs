using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static City;

public class CityDesicion
{
    public Dictionary<string, float> prices = new Dictionary<string, float>() { {"Wood",0}, { "Stone", 0 }, { "Gold", 0 }, { "Food", 0 } };
    List<CityItem> items = new List<CityItem>();
    Dictionary<BuildType, int> builds = new Dictionary<BuildType, int>();
    float[] basePrices = new float[4] { 2, 3, 1, 1 };
    float goldCoeff = 1;
    public CityDesicion(List<CityItem> items, Dictionary<BuildType, int> Builds)
    {
        this.items = items;
        this.builds = Builds;
        calculatePrices();
    }
    public void calculatePrices()
    {
        if (items == null) return;
        int length = items.Count;

        goldCoeff = 1 + (GameObject.FindObjectOfType<timeSystem>().Turns * .04f);

        foreach(var item in items)
        {
            if (item.canSell)
                calculateItemValue(item.itemName);
        }
    }
    void calculateItemValue(string itemName)
    {
        if(itemName== "Gold")
        {
            prices["Gold"] = goldCoeff; return;
        }
        if (itemName == "Food")
        {
            float CurrentCount = items.Find(x => x.itemName == "Food").count * foodConsume;
            if (CurrentCount == 0) return;
            float neededCount = items.Find(x => x.itemName == "Population").count * foodConsume;

            prices["Food"] = basePrices[3] * Mathf.Pow((neededCount / CurrentCount), 2.4f) * goldCoeff;
            return;
        }
        int index = 0;
        if (itemName == "Stone") index = 1;
        
        float currentCount = items.Find(x => x.itemName == itemName).count;
        if (currentCount == 0) return;
        float expendedCount = 0;
        foreach (var build in builds)
        {
            expendedCount += expenses[build.Key][index] * build.Value;
        }
        prices[itemName] = basePrices[index] * Mathf.Pow((expendedCount / currentCount), 1.2f) * goldCoeff;
    }
    float foodConsume=10f;

    Dictionary<BuildType, List<int>> expenses = new Dictionary<BuildType, List<int>>()
    {
        {BuildType.FarmMill, new List<int>(){100,25,0} },
        {BuildType.LumberMill,new List<int>(){50,10,0} },
        {BuildType.Quarry, new List<int>(){0,0,0 }},
        {BuildType.TradeHub,new List<int>(){0,0,0 } },
        {BuildType.House,new List<int>() {0,0,5} }
    };
}
