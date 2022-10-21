using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmMill : MonoBehaviour
{
   // [SerializeField] GameObject PlantPanel;
    FarmProductsColor farm;
    [SerializeField] FarmProduct PlantedProduct;
    [SerializeField] GameObject PlantPanel;
    private float timer;
    private float growTime;
    private float amountProduct;
    private bool waitResponse=false;
    private bool Growth = false;


    // Start is called before the first frame update
    void Start()
    {
        PlantPanel = Resources.FindObjectsOfTypeAll<plantPanel>()[0].gameObject;
        farm = GetComponentInChildren<FarmProductsColor>();
        if(PlantedProduct!=null) PlantProduct(PlantedProduct);
        //    PlantPanel.SetActive(false);
        if (Cursor.visible)
            Villagers.GiveJob(this.gameObject, (int)GetComponent<Building>().PopulationExpense);
    }

    // Update is called once per frame
    void Update()
    {
        CheckFarm();
        CheckProducts();
    }
    void CheckProducts()
    {
        if (!Growth)
            return;
        amountProduct += 0.01f;
        if (amountProduct >= 1)
        {
            amountProduct = 0;
            Inventory.instance.Add(PlantedProduct.item);
        }
    }
    void CheckFarm()
    {
        //if (waitResponse && !PlantPanel.activeSelf)
        //{
        //    PlantProduct(PlantPanel.GetComponent<PlantPanel>().AcceptedProduct);
        //    waitResponse = false;
        //}  

        if (PlantedProduct == null) return;
        timer += 0.01f;
        if (timer>=growTime)
        {
            farm.SetColor(PlantedProduct.GrowthColor);
            Growth = true;
        }
    }
    public void PlantProduct(FarmProduct newProduct)
    {
        Growth = false;
        PlantedProduct = newProduct;
        if (PlantedProduct == null) return;
        farm.SetColor(PlantedProduct.PlantedColor);
        growTime = PlantedProduct.growTime;
        timer = 0;
    }
    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        PlantPanel.transform.position =Input.mousePosition;
        PlantPanel.SetActive(true);
        PlantPanel.GetComponent<plantPanel>().farmMill = this.gameObject;
        //waitResponse = true;
    }
    
    
}
