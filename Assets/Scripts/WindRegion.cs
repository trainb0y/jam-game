using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WindRegion : MonoBehaviour
{
    [SerializeField] private Vector2 wind;

    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Rigidbody2D>().velocity += wind;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>() != null)
        {
            _audio.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<PlayerController>() != null)
        {
            _audio.Stop();
        }
    }
}