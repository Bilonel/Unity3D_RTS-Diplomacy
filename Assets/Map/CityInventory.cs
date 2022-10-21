using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityInventory
{
    public List<CityItem> list = new List<CityItem>();
    public void add(CityItem item)
    {
        foreach(var x in list)
        {
            if(x.itemName==item.itemName)
            {
                x.count += item.count;
                return;
            }
        }
        list.Add(item);
    }
    public void add(string itemName,float count)
    {
        foreach (var x in list)
        {
            if (x.itemName == itemName)
            {
                x.count += count;
                return;
            }
        }
        CityItem newItem;
        switch(itemName)
        {
            case "Wood" : newItem = new Wood(count);break;
            case "Stone": newItem = new Stone(count); break;
            case "Food": newItem = new Food(count); break;
            case "Population": newItem = new Population(count); break;
            case "Gold": newItem = new Gold(count); break;
            default:return;
        }
        list.Add(newItem);
    }
    public CityItem find(string name)
    {
        return list.Find(x => x.itemName == name);
    }
}
