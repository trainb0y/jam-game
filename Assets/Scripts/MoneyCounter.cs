using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    public int amount;
    
    // Update is called once per frame
    private void Update()
    {
        display.text = amount.ToString();
    }
}