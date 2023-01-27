using System;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float deathYLevel;

    private void Start()
    {
        transform.position = respawnPoint.position;
    }

    void Update()
    {
        if (transform.position.y < deathYLevel)
        {
            transform.position = respawnPoint.position;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }   
    }
}
