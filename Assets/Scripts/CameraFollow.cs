using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private Vector3 _velocity = Vector3.zero;
    
    void LateUpdate()
    {
        var smooth = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, speed);
        transform.position = new Vector3(smooth.x, smooth.y, transform.position.z);
    }
}
