using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float speed;
    [SerializeField] private int waitTime;
    private Vector3 _destination;
    private float _counter = 0;

    void Start()
    {
        transform.position = startPos;
        _destination = endPos;
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, speed*Time.deltaTime);
        if ((transform.position - _destination).sqrMagnitude < 0.2f) {
            _counter += Time.deltaTime;
            if (_counter > waitTime) {
                _counter = 0;
                if (endPos == _destination) _destination = startPos;
                else _destination = endPos;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            other.transform.parent = transform;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            other.transform.parent = null;
        }
    }
}