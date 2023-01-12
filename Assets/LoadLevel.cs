using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public void animEvent()
    {
        Application.LoadLevel(1);
        Application.UnloadLevel(0);
    }
}
