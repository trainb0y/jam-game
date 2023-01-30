using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WindRegion: MonoBehaviour
{
        [SerializeField] private Vector2 wind;

        void OnTriggerStay2D(Collider2D other)
        {
               other.GetComponent<Rigidbody2D>().velocity += wind;
        }
}