using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class FarmProductsColor : MonoBehaviour
{
    public MeshRenderer[] list;
    // Start is called before the first frame update
    void Start()
    {
        list = GetComponentsInChildren<MeshRenderer>();
        SetColor(Color.green);
    }
    public void SetColor(Color color)
    {
        if (list == null) return;
        foreach (var item in list)  // EACH ITEMS,
        {
            item.material.color = color;
        }
    }
}
