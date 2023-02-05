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

        _display.text = GlobalData.Instance.cashAmount.ToString();
    }
}