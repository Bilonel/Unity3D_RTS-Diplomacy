using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Quarry : MonoBehaviour
{
    public float mineCapability=1;
    public int workerCount=2;
    // Start is called before the first frame update
    void Start()
    {
        if (Cursor.visible)
            Villagers.GiveJob(this.gameObject, workerCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
