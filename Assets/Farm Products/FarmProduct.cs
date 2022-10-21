using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Farm Product",menuName ="Product/new Farm Product")]
public class FarmProduct : ScriptableObject
{
    public Item item;
    public Color PlantedColor=Color.green;
    public Color GrowthColor=Color.yellow;
    public float growTime;
    public float price;
}
