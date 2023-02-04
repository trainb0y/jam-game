using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    public int lastTime;
    
    // Update is called once per frame
    private void Update()
    {
        var time = Mathf.FloorToInt(Time.time);
        if (time > lastTime)
        {
            GlobalData.Instance.cashAmount--;
            lastTime = time;
        }
        display.text = GlobalData.Instance.cashAmount.ToString();
    }
}