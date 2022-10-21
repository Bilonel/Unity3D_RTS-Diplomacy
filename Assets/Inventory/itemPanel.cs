using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemPanel : MonoBehaviour
{
    public void Set(Item item)
    {
        GetComponentsInChildren<Image>()[1].sprite = item.icon;
        GetComponentsInChildren<Image>()[1].type = Image.Type.Simple;
        GetComponentsInChildren<Image>()[1].preserveAspect = true;
        GetComponentInChildren<Text>().text = item.Name + " x"+item.count.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
