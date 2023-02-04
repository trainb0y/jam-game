using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CashPickup : MonoBehaviour
{
    [SerializeField] private int moneyAmount = 100;
    
    private bool _shrinking = false;
    private Light2D _light;

    private void Start()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (_shrinking)
        {
            transform.localScale = new Vector3(
                transform.localScale.x - Time.deltaTime,
                transform.localScale.y - Time.deltaTime,
                transform.localScale.z
            );
            _light.intensity -= Time.deltaTime * 2;
            if (transform.localScale.x < 0.05)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var cont = col.GetComponent<PlayerController>();
        if (cont != null)
        {
            GlobalData.Instance.cashAmount += moneyAmount;
            _shrinking = true;
        }
    }
}
