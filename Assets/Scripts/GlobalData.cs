using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;
    public int cashAmount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}