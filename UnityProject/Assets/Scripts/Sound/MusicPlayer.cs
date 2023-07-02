using UnityEngine;

[RequireComponent(typeof(AkEvent))]
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField] private AK.Wwise.State musicState;

    private void Awake()
    {
        musicState.SetValue();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
