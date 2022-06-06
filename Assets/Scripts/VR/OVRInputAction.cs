using UnityEngine;
using UnityEngine.Events;

public class OVRInputAction : MonoBehaviour
{
    [SerializeField] private OVRInput.Button input;
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private void Update()
    {
        if (OVRInput.GetDown(input))
            onPressed.Invoke();
        if (OVRInput.GetUp(input))
            onReleased.Invoke();
    }
}
