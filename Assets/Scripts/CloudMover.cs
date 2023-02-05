using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(
            transform.position.x + speed * Time.deltaTime,
            transform.position.y + (Random.value - 0.5f) * Time.deltaTime,
            0
        );
    }
}