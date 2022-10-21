using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> testItems = new List<Item>();
    [SerializeField] List<Item> ItemPrefabs = new List<Item>();

    public static Inventory instance;
    public List<Item> items = new List<Item>();
    [SerializeField] GameObject Panel_Item;
    Inventory()
    {
        instance = this;
    }
    private void Start()
    {
        foreach (var item in testItems)
            Add(item, ((int)Random.Range(50, 1000)));
    }
    public bool Add(Item item, int amount = 1)
    {
        bool flag = true;
        if (item == null) return false;
        var CurrentItem = items.Find(x => x.Name == item.Name);
        if (CurrentItem==null)
        {
            if (amount > 0)
            {
                item.count = amount;
                items.Add(item);
                Instantiate(Panel_Item, transform).GetComponent<itemPanel>().Set(item);
            }
            else if (amount == 0) return true;
            else return false;
        }
        else
        {
            if (CurrentItem.count + amount < 0)
                return false;
            
            CurrentItem.count += amount;
            if (CurrentItem.count == 0)
            {
                Destroy(GetComponentsInChildren<itemPanel>()[items.IndexOf(CurrentItem)].gameObject);
                Remove(CurrentItem);
            }
            else GetComponentsInChildren<itemPanel>()[items.IndexOf(CurrentItem)].Set(CurrentItem);
        }
        
        return flag;
    }
    public bool Add(string itemName, int amount = 1)
    {
        bool flag = true;
        if (itemName == "Food") itemName = "Berries";
        var CurrentItem = items.Find(x => x.Name == itemName);
        if (CurrentItem == null)
        {
            if (amount > 0)
            {
                var item = ItemPrefabs.Find(x => x.Name == itemName);
                if (item == null) return false;
                item.count = amount;
                items.Add(item);
                Instantiate(Panel_Item, transform).GetComponent<itemPanel>().Set(item);
            }
            else if (amount == 0) return true;
            else return false;
        }
        else
        {
            if (CurrentItem.count + amount < 0)
                return false;

            CurrentItem.count += amount;
            if (CurrentItem.count == 0)
            {
                Destroy(GetComponentsInChildren<itemPanel>()[items.IndexOf(CurrentItem)].gameObject);
                Remove(CurrentItem);
            }
            else GetComponentsInChildren<itemPanel>()[items.IndexOf(CurrentItem)].Set(CurrentItem);
        }

        return flag;
    }
    public void Remove(Item item)
    {
        items.Remove(item);
    }
    
}
