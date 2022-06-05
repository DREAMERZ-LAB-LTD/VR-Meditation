using UnityEngine;
using UnityEngine.Events;

public class FadeEffectController : MonoBehaviour
{
    public UnityEvent OnFadeBegin;
    private static bool initialized = false;

    private void Awake()
    {
        if (initialized)
            OnFadeBegin.Invoke();
        initialized = true;
    }
}
