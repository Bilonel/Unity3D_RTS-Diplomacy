using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    [SerializeField] Material mat;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = mat;
    }
}
