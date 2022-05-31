using UnityEngine;

[CreateAssetMenu(fileName = "Audio Settings", menuName = "Dlab/Audio/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public bool isMute = false;
    public float volume = 1;
}
