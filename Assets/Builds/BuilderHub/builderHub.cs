using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class builderHub : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if (!this.enabled) return;
        if(GetComponent<Building>().isBuildComplete&&Cursor.visible)
        {
            Villagers.GiveJob(this.gameObject, (int)GetComponent<Building>().PopulationExpense);
            this.enabled = false;
        }
    }

    
}
