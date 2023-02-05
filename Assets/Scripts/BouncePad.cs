using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
