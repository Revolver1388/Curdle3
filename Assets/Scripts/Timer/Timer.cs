using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//RITTER
[RequireComponent(typeof(TextMeshPro))]
public class Timer : MonoBehaviour
{
    public static event Action onTimeIsUp; //subscribe to this event to know whenever the time is up

    [Range(0, 59)] [SerializeField] private int Seconds = 59;
    [Range(0, 59)] [SerializeField] private int Minutes = 59;

    public TextMeshPro textBox;

    float tempSeconds = 0;

    void Start()
    {
        tempSeconds = Seconds;
        textBox.text = CurrentTime_String(tempSeconds);
    }

    void Update()
    {
        if (AllowCountdown())
            tempSeconds -= Time.deltaTime;
        textBox.text = CurrentTime_String(tempSeconds);
    }

    string CurrentTime_String(float tempSeconds)
    {
        Seconds = (int)Mathf.Ceil(tempSeconds);
        if (Minutes > 0 && Seconds == 0) { ResetSeconds(); Minutes--; }
        else if (!AllowCountdown()) { TimeIsUp(); }

        return Minutes + " : " + Seconds;
    }

    bool AllowCountdown() => Seconds <= 0 && Minutes <= 0 ? false : true;

    public void TimeIsUp() => onTimeIsUp?.Invoke();

    void ResetSeconds() { Seconds = 59; tempSeconds = Seconds; }
}
