using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public delegate void MuteStatus(bool mute);
    public delegate void VolumeStatus(float volume);
    public static MuteStatus OnChangeMute;
    public static VolumeStatus OnChangeVolume;

    public AudioSettings audioSettings;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnMute;
    [SerializeField] private UnityEvent OnUnMute;
    [SerializeField] private UnityEvent OnVolumeChanged;
    private void Start()
    {
        if (audioSettings)
        {
            Mute(audioSettings.isMute);
            SetVolume(audioSettings.volume);
        }
    }


    public void Mute(bool mute)
    {
        if (audioSettings == null)
            return;

        audioSettings.isMute = mute;
        if (OnChangeMute != null)
            OnChangeMute.Invoke(mute);

        if (mute)
            OnMute.Invoke();
        else
            OnUnMute.Invoke();
    }


    public void SetVolume(float volume)
    {
        audioSettings.volume = volume;
        if (OnChangeVolume != null)
            OnChangeVolume.Invoke(volume);

        OnVolumeChanged.Invoke();
    }

}
