using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    Animator anim;
    float lastTime;
    Vector3 positionInWorkplace=Vector3.one*-1;
    int counterForWork = 0;
    villagerMovement villager;
    // Update is called once per frame
    void Start()
    {
        anim = GetComponent<Animator>();
        villager = GetComponent<villagerMovement>();
    }
    public void Work()
    {
        if (anim.GetBool("isFarming"))
        {
            if (Time.time - lastTime >= 20)
            {
                anim.SetBool("isFarming", false);
                positionInWorkplace = villager.workplace.transform.position;
                counterForWork++;
            }
        }
        else
        {
            if (positionInWorkplace.y < 0)
            {
                var farms = villager.workplace.GetComponentInChildren<FarmProductsColor>().list;
                int index = Random.Range(0, farms.Length - 1);
                positionInWorkplace = farms[index].gameObject.transform.position;
                positionInWorkplace.y = 0;
            }
            else
            {
                villager.MoveTo(positionInWorkplace);
                if (isArrive(positionInWorkplace))
                {
                    if (positionInWorkplace == villager.workplace.transform.position)
                        positionInWorkplace = Vector3.one * -1;
                    else
                    {
                        anim.SetBool("isFarming", true);
                        lastTime = Time.time;
                    }
                }
            }
        }
        if (counterForWork > 4)
        {
            counterForWork = 0;
            villager.steps++;
        }
    }
    public bool isArrive(Vector3 target, float range = .2f)
    {
        return (target - transform.position).magnitude < range;
    }
}
