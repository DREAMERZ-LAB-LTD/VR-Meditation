using UnityEngine;
using UnityEngine.Events;

public class GuideInitilaizer : MonoBehaviour
{
    private static bool initialized = false;

    [SerializeField] UnityEvent OnIncludeGuide;
    [SerializeField] UnityEvent OnWithOutGuide;

    private void Awake()
    {
        if (initialized)
            OnWithOutGuide.Invoke();
        else
        { 
            initialized = true;
            OnIncludeGuide.Invoke();
        }
    }
}
