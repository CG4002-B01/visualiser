using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerOpp : MonoBehaviour
{
    const float ShieldTimer = 10;
    float timeRemaining;
    bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        startTimer = false;
        timeRemaining = ShieldTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0 && startTimer)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    public void SetStartTimer(bool status)
    {
        startTimer = status;
        if (status == false)
        {
            timeRemaining = 10;
        }
    }

    public float GetTime()
    {
        return timeRemaining;
    }
}
