using System.Collections;
using UnityEngine;

public class ScreenShakeManager: MonoBehaviour
{
    private float _duration = 1f;
    private AnimationCurve _curve;
    
    public void Shake(float duration, AnimationCurve strength)
    {
        _duration = duration;
        _curve = strength;
            
        StartCoroutine(Shaking());
    }
    
    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            transform.position = startPos + Random.insideUnitSphere * _curve.Evaluate(elapsed / _duration);
            yield return null;
        }

        transform.position = startPos;
    }
}
