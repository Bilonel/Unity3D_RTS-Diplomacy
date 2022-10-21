using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public MeshRenderer grid;
    
    public GameObject activeBuild;
    public bool validPositioning;
    bool isBuildEvenX;
    bool isBuildEvenZ;
    float tileSize = 1f;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (!grid.enabled) return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 350,LayerMask.GetMask("Grid")))
        {
            Vector3 pos = hit.point;
            pos.y = 0.1f;
            pos.x = Mathf.RoundToInt(pos.x / tileSize) * tileSize;
            pos.z = Mathf.RoundToInt(pos.z / tileSize) * tileSize;
            if (!isBuildEvenX) pos.x -= (tileSize / 2);
            if (!isBuildEvenZ) pos.z -= (tileSize / 2);
            transform.position = pos;
        }
    }
    //private void LateUpdate()
    //{
    //    if (activeBuild != null)
    //        if (!GameObject.Find("base").GetComponent<Base>().CanBuySomething(activeBuild.GoldExpense))
    //            GetComponentInChildren<Placement>().turnRed();
    //}
    public void activateCursor(GameObject build)
    {
        if (build == null)
            return;
        gameObject.SetActive(true);
        Cursor.visible=false;

        grid.enabled = true;
        isBuildEvenX = (build.GetComponent<Building>().BuildingSize.x / tileSize)%2==0;
        isBuildEvenZ = (build.GetComponent<Building>().BuildingSize.y / tileSize) % 2 == 0;

        //activeBuild = build.GetComponentInChildren<Building>();
        activeBuild = Instantiate(build,this.transform);
        activeBuild.GetComponent<MeshRenderer>().enabled = true;
        activeBuild.transform.localPosition = Vector3.zero;
        Outline outlineScript;
        activeBuild.TryGetComponent(out outlineScript);
        if (outlineScript!=null) outlineScript.enabled = false;
        //var placement=Instantiate(build.transform.Find("Placement").gameObject, this.transform);
        //transform.localScale = activeBuild.scale;
        //placement.transform.localScale = new Vector3(placement.transform.localScale.x/ transform.localScale.x, placement.transform.localScale.y / transform.localScale.y,1);
        //placement.SetActive(true);
        //build.transform.Find("Placement").gameObject.SetActive(true);
    }
}
