using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;
    // Start is called before the first frame update
    void Start()
    {
        TargetObject.GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }
}
