using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soldier1BuyPanel : MonoBehaviour
{
   // [SerializeField] barrac militaryBase;
    //private Scrollbar scroll;
    //private Text scrollText;
    //private Button buyButton;
    //private Text buttonText;
    //private int Count;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    GetComponentInChildren<Image>().GetComponentInChildren<Text>().text ="$"+ militaryBase.Soldier1_price.ToString();
    //    scroll = GetComponentInChildren<Scrollbar>();
    //    scrollText= scroll.GetComponentInChildren<Text>();
    //    buyButton = GetComponentInChildren<Button>();
    //    buttonText = buyButton.GetComponentInChildren<Text>();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    //public void OnScroll()
    //{
    //    Count = (int)(scroll.value * 19)+1;
    //    scrollText.text="x"+Count.ToString();
    //    buttonText.text = "BUY " + scrollText.text;
    //    buttonText.color = Color.white;
    //}
    //public void OnClickButton()
    //{
    //    float price = Count * militaryBase.Soldier1_price;
    //    if (GameObject.Find("base").GetComponent<Base>().buySomething(price))
    //        militaryBase.buySoldier1(Count);
    //    else
    //    {
    //        buttonText.text = "No Coin";
    //        buttonText.color = Color.red;
    //    }
    //}
}
