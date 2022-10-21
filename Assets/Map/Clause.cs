using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clause : MonoBehaviour
{
    TradePanel parentPanel;

    [SerializeField] Text text1;
    [SerializeField] Text text2;
    [SerializeField] InputField input;

    bool isGiven;
    int index;
    public string itemName;
    public int count;
    int maxCount;
    string verb;
    string preposition;
    string cityName;
    public void Set(string itemName, int Itemcount, int maxCount, bool isGiven, string cityname,int index)
    {
        this.maxCount = maxCount;
        parentPanel = GameObject.FindObjectOfType<TradePanel>();
        this.index = index;
        input.onValueChanged.AddListener(delegate { submit(); });
        this.itemName = itemName;
        count = Itemcount;
        cityName = cityname.ToUpper();
        this.isGiven = isGiven;
        verb = "GIVES"; preposition = "TO";
        if (!isGiven)
        {
            verb = "TAKES"; preposition = "FROM";
            input.GetComponent<Image>().color = new Color(165f/255, 1, 165f/255, 1);
        }
        setText();
    }
    private void submit()
    {
        int diff = count;
        int.TryParse(input.text, out count);

        if (count < 0) count = 0;
        else if (count > maxCount) count = maxCount; 

        diff -= count;

        parentPanel.updateButton(isGiven, index, diff);
        setText();
    }
    public void updateCount(int count)
    {
        this.count += count;
        setText();
    }
    private void setText()
    {
        text1.text = "WE " + verb + " ";
        input.text = count.ToString();
        text2.text = itemName.ToUpper() + " " + preposition + " " + cityName;
    }
}
