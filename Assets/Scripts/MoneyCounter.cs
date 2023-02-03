using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    // Update is called once per frame
    private void Update()
    {
        display.text = Mathf.RoundToInt(Time.time).ToString();
    }
}