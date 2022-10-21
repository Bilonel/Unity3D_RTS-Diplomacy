using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.SearchService;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] diplomacy diplomacyPanel;
    Vector2 resolution = new Vector2(1920, 1080);
    float MoveSpeed = 4000f;
    Vector3 defaultScale=Vector3.one;

    float zoom;
    float horizontal;
    float vertical;

    timeSystem timeSys;
    public void Activate()
    {
        if (timeSys == null) timeSys = GameObject.FindObjectOfType<timeSystem>();
        timeSys.onClickSpeed(0);
    }
    public void Deactivate()
    {
        timeSys.onClickSpeed(1);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        getInput();
        MapMove();
        Scroll();
        ScrollBack();
    }
    void getInput()
    {
        zoom= Input.mouseScrollDelta.y;
        //horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        Vector2 pos = Input.mousePosition;
        if (pos.x < 10) horizontal = -1;
        else if (pos.x > resolution.x - 10) horizontal = 1;
        else horizontal = 0;
        if (pos.y < 10) vertical = -1;
        else if (pos.y > resolution.y - 10) vertical = 1;
        else vertical = 0;
    }
    void Scroll()
    {
        if (zoom == 0) return;
        float scaleX = GetComponent<RectTransform>().localScale.x;
        float newScale = scaleX + zoom *Time.deltaTime*25;
        if (newScale < 1) return;
        if (newScale > 6) return;
        GetComponent<RectTransform>().localScale = Vector3.one * newScale;
        //ScrollMove();
    }
    void ScrollBack()
    {
        if (zoom >= 0) return;
        if (GetComponent<RectTransform>().localScale.x <= 2) return;
        
        float coeff= zoom /(GetComponent<RectTransform>().localScale.x - 2);
        RectTransform rect = GetComponent<RectTransform>();

        Vector2 Shift = resolution / 2 - (Vector2)rect.position;
        float dX = Shift.x * coeff;
        float dY = Shift.y * coeff;
        Vector3 newPosition = new Vector3(rect.position.x + dX * Time.deltaTime * MoveSpeed, rect.position.y + dY * Time.deltaTime * MoveSpeed, rect.position.z);
            rect.position = newPosition;
    }
    private void ScrollMove()
    {
        Vector2 targetPos;
        if (zoom > 0) targetPos = (Vector2)Input.mousePosition- resolution/2;
        else targetPos = (Vector2)GetComponent<RectTransform>().position - resolution / 2;

        if (Math.Abs(targetPos.x) > 5) targetPos.x = Mathf.Sign(targetPos.x) * Time.deltaTime * MoveSpeed;
        else targetPos.x = 0;
        if (Math.Abs(targetPos.y) > 5) targetPos.y = Mathf.Sign(targetPos.y) * Time.deltaTime * MoveSpeed;
        else targetPos.y = 0;
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 newPosition = new Vector3(rect.position.x - targetPos.x, rect.position.y - targetPos.y, rect.position.z);
        if (checkPos(newPosition))
            rect.position = newPosition;
        else
            print("ERRRRRR");
    }
    void MapMove()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 newPosition = new Vector3(rect.position.x - horizontal*Time.deltaTime*MoveSpeed, rect.position.y - vertical * Time.deltaTime * MoveSpeed, rect.position.z);
        if (checkPos(newPosition))
            rect.position = newPosition;
    }

    bool checkPos(Vector2 Position)
    {
        Vector2 currentSize = GetComponent<RectTransform>().localScale * resolution;
        currentSize /= 2;
        bool left = Position.x - currentSize.x > 0;
        bool right = Position.x + currentSize.x < resolution.x;

        bool up = Position.y+currentSize.y < resolution.y;
        bool down = Position.y - currentSize.y > 0;
        return !left && !right && !up && !down;
    }
    public void city_button(City city)
    {
        diplomacyPanel.gameObject.SetActive(true);
        diplomacyPanel.setCity(city);
    }
}
