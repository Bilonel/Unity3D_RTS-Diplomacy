using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    public bool storable = true;
    static int id_s;
    public int id;
    public string Name;
    public float price;
    public float count;
    public Sprite icon;

    private void OnEnable()
    {
        count = 1;
        if (storable)
            id = id_s++;
        else id = 0;
    }
    private void OnDestroy()
    {
        id_s--;
    }
    static Item()
    {
        id_s = 1;
    }
}
