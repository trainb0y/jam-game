using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class PlayerMovement: ScriptableObject
{
    [SerializeField] public AnimationCurve jumpCurve;
    [SerializeField] public AnimationCurve accelerationCurve;
    public float fallSpeed;
    public float idleCutoff;
}