using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberMill : MonoBehaviour
{
    [SerializeField] public float SizeOfArea;
    [SerializeField] public GameObject saplingPrefab;
    public Transform SaplingsParent;
    // Start is called before the first frame update
    void Start()
    {
        if(Cursor.visible)
            Villagers.GiveJob(this.gameObject, 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(SizeOfArea,0.1f,SizeOfArea));
    }
}
