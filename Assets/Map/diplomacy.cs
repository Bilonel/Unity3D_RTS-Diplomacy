using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class diplomacy : MonoBehaviour
{
    [SerializeField] public Image cityFlag;
    [SerializeField] public Text nameLabel;
    [SerializeField] public List<Text> itemTexts = new List<Text>();
    [SerializeField] public Text buildsLabel;

    [SerializeField] private TradePanel tradePanel;

    City currentCity;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void exit_button()
    {
        gameObject.SetActive(false);
    }
    public void setCity(City city)
    {
        if (city == null) return;
        currentCity = city;
        currentCity.Refresh();

        cityFlag.color = currentCity.color;
        nameLabel.text = currentCity.CityName;

        for (int i = 0; i < itemTexts.Count; i++)
        {
            CityItem currentItem = currentCity.inventory.list[i];
            itemTexts[i].text = currentItem.itemName + " : " + currentItem.count.ToString();
        }

        buildsLabel.text = "BUILD LIST : \n";
        foreach (var item in currentCity.Builds)
        {
            buildsLabel.text += "  " + item.Key.ToString() + " : " + item.Value.ToString() + " \n";
        }
    }

    public void TradePanelButton()
    {
        if (currentCity == null) return;
        tradePanel.gameObject.SetActive(true);
        tradePanel.setCities(currentCity);
    }
}
