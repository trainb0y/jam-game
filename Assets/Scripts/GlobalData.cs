using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public int cashAmount;
    
    public static GlobalData Instance;
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
