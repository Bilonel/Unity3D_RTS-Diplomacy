using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeSystem : MonoBehaviour
{
    [SerializeField] Sprite daySprite;
    [SerializeField] Sprite nightSprite;
    [SerializeField] float DayAnimationTimeSec;
    [SerializeField] float NightAnimationTimeSec;
    float lastTime;
    public bool isDay=false;
    float AnimationTimeSec=1;

    float speed = 1f;
    public int Turns = 0;
    // Start is called before the first frame update
    void Start()
    {
        SwitchDayNight();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTime();
        CheckSpeed();
    }
    void CheckTime()
    {
        float amount=GetComponentsInChildren<Image>()[2].fillAmount = (Time.time-lastTime)/ AnimationTimeSec;
        if (amount >= 1)
            SwitchDayNight();


    }
    void SwitchDayNight()
    {
        isDay = !isDay;
        if (isDay) TurnDay();
        else TurnNight();
        lastTime = Time.time;
    }
    void TurnDay()
    {
        GetComponentsInChildren<Image>()[1].sprite = daySprite;
        GetComponentsInChildren<Image>()[2].sprite = daySprite;
        AnimationTimeSec = DayAnimationTimeSec;
        Turns++;
    }
    void TurnNight()
    {
        GetComponentsInChildren<Image>()[1].sprite = nightSprite;
        GetComponentsInChildren<Image>()[2].sprite = nightSprite;
        AnimationTimeSec = NightAnimationTimeSec;
    }

    void CheckSpeed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            speed = 0;

        Time.timeScale = speed;
    }
    public void onClickSpeed(float speed)
    {
        print(speed);
        this.speed = speed;
    }
}
