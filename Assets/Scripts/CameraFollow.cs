using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Vector3 _velocity = Vector3.zero;
    
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime, 10);
    }
}
