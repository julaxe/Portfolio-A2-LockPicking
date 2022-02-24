using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    public int timer;

    private float _counter;
    private TMPro.TextMeshProUGUI _uiTimer;

    private void Start()
    {
        _uiTimer = transform.Find("Counter").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        //update timer first.
        UpdateTimer();
        UpdateColor();
        //update UI
        _uiTimer.text = timer.ToString();
    }

    private void UpdateTimer()
    {
        if (timer <= 0) return;
        if (_counter >= 1.0f)
        {
            timer -= 1;
            _counter = 0.0f;
        }
        _counter += Time.deltaTime;
    }

    private void UpdateColor()
    {
        if (timer < 20 && timer > 10)
        {
            _uiTimer.color = Color.yellow;
        }
        else if (timer <= 10)
        {
            _uiTimer.color = Color.red;
        }
    }
}
