using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.ProBuilder;
public class Building : MonoBehaviour
{
    public int PopulationExpense=0;
    //public GameObject buildSprite { get; private set; }
    [SerializeField] GameObject BuildMesh;
    [SerializeField] GameObject Body;
    [SerializeField] float BuildingTime;
    Grid grid;
    private float currentBuildingCompletion = 0;

    public Vector2 scale;
    public bool placement = true;
    int collisionObjectCount=0;
    public Vector2 BuildingSize;
    public bool isBuildComplete = true;
    public string type;
    // Start is called before the first frame update
    void Start()
    {
        
        grid = GameObject.FindObjectOfType<Grid>();
        SetColor(new Color(0, 1, 0, 0.5f)); // PLACEMENTS INIT COLOR
        if (scale==Vector2.zero) scale = transform.localScale;

        if (BuildMesh == null) return;
        if (Cursor.visible)
        { 
            BuildList.builds.Add(gameObject);
            BuildMesh.SetActive(true);
            Body.SetActive(false);
            isBuildComplete = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        checkPlacement();
        checkBuildingProcess();
    }
    void checkBuildingProcess()
    {
        if (isBuildComplete) return;
        if (currentBuildingCompletion > BuildingTime)
        {
            BuildList.builds.Remove(gameObject);
            isBuildComplete = true;
            BuildMesh.SetActive(false);
            Body.SetActive(true);
            if (type == "FarmMill")
                GetComponent<FarmMill>().enabled = true;
            else if (type == "LumberMill")
                GetComponent<LumberMill>().enabled = true;
            this.enabled = false;
            Destroy(BuildMesh);
        }
    }
    public void Build(float amount)
    {
        currentBuildingCompletion += amount;
    }
    void checkPlacement()
    {
        if (!GetComponent<MeshRenderer>().enabled) return;
        placement = collisionObjectCount <= 0 && !grid.isWater(new Vector2(transform.position.x,transform.position.z),BuildingSize);
        if (placement)
        {
            SetColor(new Color(0, 1, 0, 0.5f));
            collisionObjectCount = 0;
        }
        else SetColor(new Color(1, 0, 0, 0.5f));
    }
    public void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) != "Build") return;
        
        collisionObjectCount++;
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionObjectCount--;
    }
}
