using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public float Radius;
    public LayerMask Layer;
    public bool collectable=false;
    public List<GameObject> Resources = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        FindResources();
        if (Cursor.visible)
            Villagers.GiveJob(gameObject, -1, false);
    }
    void FindResources()
    {
        Resources.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius, Layer);
        foreach (var item in colliders)
            Resources.Add(item.gameObject);
    }
    public GameObject getResource()
    {
        if (Resources.Count < 1)
        {
            if (collectable)
                FindResources();
            else
                return null;
        }
        GameObject res= Resources[0];
        Resources.RemoveAt(0);
        return res;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
