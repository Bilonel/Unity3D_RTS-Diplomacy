using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class CityItem
{
    public string itemName;
    public Sprite Icon;
    public float count;
    public bool canSell = true;
}
public class Wood:CityItem
{
    public Wood(float count)
    {
        itemName = "Wood";
        Icon= Resources.Load<Sprite>(itemName+"_icon");
        this.count = count;
    }
}
public class Stone : CityItem
{
    public Stone(float count)
    {
        itemName = "Stone";
        Icon = Resources.Load<Sprite>(itemName + "_icon");
        this.count = count;
    }
}
public class Food : CityItem
{
    public Food(float count)
    {
        itemName = "Food";
        Icon = Resources.Load<Sprite>(itemName + "_icon");
        this.count = count;
    }
}
public class Population : CityItem
{
    public Population(float count)
    {
        itemName = "Population";
        Icon = Resources.Load<Sprite>(itemName + "_icon");
        this.count = count;
        canSell = false;
    }
}
public class Gold : CityItem
{
    public Gold(float count)
    {
        itemName = "Gold";
        Icon = Resources.Load<Sprite>(itemName + "_icon");
        this.count = count;
    }
}