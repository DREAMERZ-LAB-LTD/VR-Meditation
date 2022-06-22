using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class AudioEndingResponse : MonoBehaviour
{
    private AudioSource audioSource = null;
    public UnityEvent OnEnd;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(OnTrackEnd());
    }

    private IEnumerator OnTrackEnd()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
            if (audioSource.playOnAwake)
            {
                var trackDuration = audioSource.clip.length;
                yield return new WaitForSeconds(trackDuration);
                
                OnEnd.Invoke();
            }
    }
}



