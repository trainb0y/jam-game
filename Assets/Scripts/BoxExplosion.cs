using UnityEngine;

public class BoxExplosion : MonoBehaviour
{
    [SerializeField] private float threshold;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.relativeVelocity.magnitude > threshold)
        {
            var p = GetComponent<ParticleSystem>();
            p.Stop();
            p.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 4);
            GlobalData.Instance.cashAmount += 10;
            GetComponent<AudioSource>().Play();
        }
    }
}