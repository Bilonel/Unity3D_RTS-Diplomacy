using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TradePanel : MonoBehaviour
{
    [SerializeField] Button itemPrefab;
    [SerializeField] public GameObject Inventory1;
    [SerializeField] public GameObject Inventory2;
    [SerializeField] GameObject clausePrefab;
    [SerializeField] RectTransform clausePanel;

    City CurrentCity;
    List<Clause> clauses1 = new List<Clause>();
    List<Clause> clauses2 = new List<Clause>();
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCities(City city)
    {
        if (city == null) return;
        CurrentCity = city;
        foreach(var cityItem in CurrentCity.inventory.list)
        {
            if(cityItem.count>0 && cityItem.canSell)
            {
                Button newButton = Instantiate(itemPrefab, Inventory2.transform); 
                newButton.GetComponentInChildren<Text>().text = cityItem.count.ToString();
                newButton.GetComponentsInChildren<Image>()[1].sprite = cityItem.Icon;
                newButton.onClick.AddListener(delegate { Item_Button(cityItem.itemName,((int)cityItem.count), false, newButton); });
            }
        }
        foreach (var Item in Inventory.instance.items)
        {
            if (Item.count > 0)
            {
                Button newButton = Instantiate(itemPrefab, Inventory1.transform);
                newButton.GetComponentInChildren<Text>().text = Item.count.ToString();
                newButton.GetComponentsInChildren<Image>()[1].sprite = Item.icon;
                newButton.onClick.AddListener(delegate { Item_Button(Item.Name,((int)Item.count), true, newButton); });
            }
        }
    }

    public void Item_Button(string itemName, int maxCount, bool isGiven,Button btn)
    {
        int Count = int.Parse(btn.GetComponentInChildren<Text>().text);
        if(Count==0)
        {
            btn.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
            return;
        }
        int unitSize = 1;
        if (Input.GetKey(KeyCode.LeftShift)) unitSize = 10;
        else if (Input.GetKey(KeyCode.LeftControl)) unitSize = 5;
        if (unitSize > Count) unitSize = Count;

        if (clauses1.Count+clauses2.Count >= 6) clausePanel.sizeDelta += new Vector2(0,40);

        print(clauses1.Count + clauses2.Count);
        int index=0;
        if(isGiven)
        {
            int i = 0;
            foreach (var item in Inventory1.GetComponentsInChildren<Button>())
            {
                if (item == btn)
                {
                    index = i;
                    break;
                }
                i++;
            }
        }
        else
        {
            int i = 0;
            foreach (var item in Inventory2.GetComponentsInChildren<Button>())
            {
                if (item == btn)
                {
                    index = i;
                    break;
                }
                i++;
            }
        }

        AddClause(itemName, unitSize, maxCount, isGiven,index);
        int currentCount = int.Parse(btn.GetComponentInChildren<Text>().text) - unitSize;
        btn.GetComponentInChildren<Text>().text = currentCount.ToString();
        updateButtonColor(btn, currentCount, isGiven);
        GetComponentInChildren<Scrollbar>().value = 0;

        Decide();
    }
    void AddClause(string itemName, int Itemcount, int maxCount, bool isGiven,int index)
    {
        if(isGiven)
        {
            foreach(var item in clauses1)
                if(item.itemName==itemName)
                {
                    item.updateCount(Itemcount); return;
                }
            Clause clause = Instantiate(clausePrefab, clausePanel).GetComponent<Clause>();
            clause.Set(itemName, Itemcount, maxCount, isGiven, CurrentCity.CityName, index);
            clauses1.Add(clause);
        }
        else
        {
            foreach (var item in clauses2)
                if (item.itemName == itemName)
                {
                    item.updateCount(Itemcount); return;
                }
            Clause clause = Instantiate(clausePrefab, clausePanel).GetComponent<Clause>();
            clause.Set(itemName, Itemcount, maxCount, isGiven, CurrentCity.CityName,index);
            clauses2.Add(clause);
        }
    }

    public void updateButton(bool isGiven,int index,int diff)
    {
        Button btn;

        if (isGiven) btn= Inventory1.GetComponentsInChildren<Button>()[index];
        else btn = Inventory2.GetComponentsInChildren<Button>()[index];
        
        int newCount = int.Parse(btn.GetComponentInChildren<Text>().text) + diff;
        
        btn.GetComponentInChildren<Text>().text = newCount.ToString();
        updateButtonColor(btn, newCount, isGiven);
    }
    void updateButtonColor(Button btn,int count,bool isGiven)
    {
        if(count==0)
            btn.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
        else
        {
            if(isGiven)
                btn.GetComponent<Image>().color = new Color(1, 0.6470588235f, 0.6470588235f, 1);
            else
                btn.GetComponent<Image>().color = new Color(0.6470588235f, 1, 0.6470588235f, 1);
        }
    }

    public void DealButton()
    {
        foreach(var item in clauses2)
        {
            Inventory.instance.Add(item.itemName, item.count);
            CurrentCity.inventory.add(item.itemName, -item.count);
        }
        foreach (var item in clauses1)
        {
            Inventory.instance.Add(item.itemName, -item.count);
            CurrentCity.inventory.add(item.itemName, item.count);
        }


        GetComponentInParent<diplomacy>().setCity(CurrentCity);

        foreach(RectTransform item in Inventory1.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (RectTransform item in Inventory2.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (RectTransform item in clausePanel.transform)
        {
            Destroy(item.gameObject);
        }
        clauses1.Clear();
        clauses2.Clear();

        CurrentCity = null;
        gameObject.SetActive(false);
    }

    void Decide()
    {
        bool response = false;
        var prices = CurrentCity.desicion.prices;
        float totalProfit = 0;
        foreach (var item in clauses1)   // PLUS
        {
            totalProfit += prices[item.itemName] * item.count;
        }
        foreach (var item in clauses2)   // PLUS
        {
            totalProfit -= prices[item.itemName] * item.count;
        }
        response = totalProfit >= 0;
        print(response + "   :"+totalProfit);
    }
}
