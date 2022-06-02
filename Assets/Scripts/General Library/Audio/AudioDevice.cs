using UnityEngine;

namespace GeneralLibrary
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioDevice : MonoBehaviour
    {
        private AudioSource audioSource;
        public static AudioSettings currentSettings = null;
        private float initialVolume;

#if UNITY_EDITOR
        [Header("Debugger")]
        private bool useDebug = false;
#endif
        private void OnEnable()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (currentSettings == null)
            {
                var audioManager = FindObjectOfType<AudioManager>();
                if (audioManager)
                    currentSettings = audioManager.audioSettings;
            }

            initialVolume = audioSource.volume;

            if (currentSettings)
            {
                OnMute(currentSettings.isMute);
                OnChangeVolume(currentSettings.volume);
            }
            else
            {
#if UNITY_EDITOR
                if (useDebug)
                    Debug.Log("<color=cyan>Audio Settings Is Null</color>");
#endif
            }


            AudioManager.OnChangeMute += OnMute;
            AudioManager.OnChangeVolume += OnChangeVolume;
        }

        private void OnDisable()
        {
            AudioManager.OnChangeMute -= OnMute;
            AudioManager.OnChangeVolume -= OnChangeVolume;
        }

        public void Play()
        {
            audioSource.Play();
        }

        private void OnMute(bool mute)
        {
            if (audioSource)
                audioSource.mute = mute;
        }


        private void OnChangeVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0, 1);
            if (audioSource)
                audioSource.volume = volume * initialVolume;
        }
    }
}