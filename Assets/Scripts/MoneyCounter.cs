using System;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    public int lastTime;
    private TextMeshProUGUI _display;

    private void Start()
    {
        _display = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        var time = Mathf.FloorToInt(Time.time);
        if (time > lastTime)
        {
            GlobalData.Instance.cashAmount--;
            lastTime = time;
        }

        try
        {
            _display.text = GlobalData.Instance.cashAmount.ToString();
        }
        catch (NullReferenceException)
        {
            // hacky fix so we can actually export and submit this thing.
        }
    }
}