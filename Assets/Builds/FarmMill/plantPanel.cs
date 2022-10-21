using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class plantPanel : MonoBehaviour
{
    public GameObject farmMill;
    FarmProduct SelectedProduct;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Activate()
    {

    }


    public void ProductClick(FarmProduct product)
    {
        SelectedProduct = product;
    }
    public void PlantClick()
    {
        if (SelectedProduct == null || farmMill == null) return;
        farmMill.GetComponent<FarmMill>().PlantProduct(SelectedProduct);
    }
    public void exitClick()
    {
        SelectedProduct = null;
        farmMill = null;
        gameObject.SetActive(false);
    }
}
