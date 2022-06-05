using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class AudioEndingResponse : MonoBehaviour
{
    private AudioSource audioSource = null;
    public UnityEvent OnEnd;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
            if (audioSource.playOnAwake)
            { 
                var totalTrack = audioSource.clip.length;
                Invoke(nameof(OnTrackEnd), totalTrack);
            }   
    }

    private void OnTrackEnd() => OnEnd.Invoke();
}



