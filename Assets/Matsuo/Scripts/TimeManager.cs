using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Header("タイマーテキスト")] private Text _timerText;
    public bool canCountTime = false;
    public float time;
    public int minute;
    public void ResetTime() => time = 0;

    System.Action<float> OnUpdateTime;



    void Start()
    {
        ResetTime();
    }

    void Update()
    {
        if (canCountTime)
        {
            time += Time.deltaTime;
            OnUpdateTime?.Invoke(time);
        }

        if (time > 60f)
        {
            minute += 1;
            time = 0;
        }
        _timerText.text =  minute.ToString() + ":" + time.ToString("00");
    }
}
