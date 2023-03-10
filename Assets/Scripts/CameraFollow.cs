using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 minBound;
    [SerializeField] private Vector3 maxBound;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    public float screenshakeUntil;
    public float screenshakeIntensityMultiplier;

    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        var smooth = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, speed);
        transform.position = new Vector3(
            Mathf.Clamp(smooth.x, minBound.x, maxBound.x),
            Mathf.Clamp(smooth.y, minBound.y, maxBound.y),
            transform.position.z
        );

        if (screenshakeUntil > Time.time)
        {
            var pos = Random.insideUnitCircle * screenshakeIntensityMultiplier;
            transform.position += new Vector3(pos.x, pos.y, 0);
        }
    }
}