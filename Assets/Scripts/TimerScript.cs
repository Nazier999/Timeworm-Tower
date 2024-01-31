using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float timeCounting;
    int minutes;
    int seconds;
    int miliseconds;
    public TMP_Text timer;
    //public GameManager Gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CountUp();
    }

    void CountUp()
    {
        timeCounting += Time.deltaTime;
        minutes = Mathf.FloorToInt(timeCounting / 60f);
        seconds = Mathf.FloorToInt(timeCounting % 60f);

        if (seconds < 10)
        {
            timer.text = minutes + ":0" + seconds;
        }
        else
        {
            timer.text = minutes + ":" + seconds;
        }
    }
}
