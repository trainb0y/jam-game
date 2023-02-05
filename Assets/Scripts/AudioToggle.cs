using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    private bool _muted = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void OnClick()
    {
        _muted = !_muted;
        GetComponent<Image>().sprite = _muted ? off : on;
        AudioListener.volume = _muted ? 0 : 1;
    }
}
