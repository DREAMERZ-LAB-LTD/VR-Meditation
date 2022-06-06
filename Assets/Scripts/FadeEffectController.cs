using UnityEngine;
using UnityEngine.Events;

public class FadeEffectController : MonoBehaviour
{
    public UnityEvent OnFadeBegin;
    private static bool initialized = true;

    private void Start()
    {
        if (initialized)
            OnFadeBegin.Invoke();
        initialized = true;
    }
}
