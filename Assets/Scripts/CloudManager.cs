using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField] private double cutoff;
    [SerializeField] private float yLevel;
    [SerializeField] private GameObject cloudPrefab;

    private void Update()
    {
        if (Random.value * Time.deltaTime > cutoff) // todo: wrong place for deltatime
        {
            var n = Instantiate(cloudPrefab);
            n.transform.position = new Vector3(
                Camera.main.transform.position.x - 30,
                yLevel + Random.value * 2,
                0
            );
            n.GetComponent<CloudMover>().speed = Random.value * 3;
            n.transform.SetParent(gameObject.transform);
        }

        foreach (var child in GetComponentsInChildren<CloudMover>())
            if (child.transform.position.x > Camera.main.transform.position.x + 30)
            {
                Destroy(child);
                print("killed a child");
            }
    }
}