using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class yesNo : MonoBehaviour
{
    public bool response;
    private Text text;
    private void Start()
    {
        text = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }
    public void YesClick()
    {
        response = true;
        gameObject.SetActive(false);
    }
    public void NoClick()
    {
        response = false;
        gameObject.SetActive(false);
    }
    public void ShowYesNoQuestion(string question)
    {
        text.text = question;
        gameObject.SetActive(true);
    }
}
