using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    villagerMovement villager;
    GameObject target;
    Flag flag;
    int step = 0;
    bool cleaned = false;
    float damage = .1f;
    // Start is called before the first frame update
    void Start()
    {
        villager = GetComponent<villagerMovement>();
        reset();
    }
    public void reset()
    {
        flag = villager.workplace.GetComponent<Flag>();
        cleaned = false;
        step = 0;
        target = null;
    }
    // Update is called once per frame
    public void Work()
    {
        switch (step)
        {
            case 0:
                ReturnHub();
                break;
            case 1:
                Clean();
                break;

            default: break;
        }
    }
    void ReturnHub()
    {
        villager.MoveTo(villager.workplace.transform.position);
        if (isArrive(villager.workplace.transform.position, .6f)) step = 1;
    }
    void Clean()
    {
        if (target == null)
        {
            target = flag.getResource();
            if (target == null)
                WorkDone();
        }
        else if (isArrive(target.transform.position, 1.5f))
        {
            if (!target.GetComponent<resource>().Damage(damage))
            {
                target = null;
                step=0;
            }
        }
        else
        {
            villager.MoveTo(target.transform.position);
        }
    }
    bool isArrive(Vector3 pos, float range = 1f)
    {
        return (pos - transform.position).magnitude < range;
    }
    void WorkDone()
    {
        villager.setWorkplace(null);
        Destroy(GetComponent<Collecter>());
    }
}
