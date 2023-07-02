using UnityEngine;


public class VolumeInitializer : MonoBehaviour
{
    private void Awake()
    {
        InitializeRtpc("MasterVolume");
        InitializeRtpc("MusicVolume");
        InitializeRtpc("SfxVolume");
    }

    private void InitializeRtpc(string rtpcName)
    {
        AkSoundEngine.SetRTPCValue(rtpcName, PlayerPrefs.GetFloat(rtpcName, 75));
    }
}