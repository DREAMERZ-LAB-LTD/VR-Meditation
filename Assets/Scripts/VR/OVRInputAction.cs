using UnityEngine;
using UnityEngine.Events;

public class OVRInputAction : MonoBehaviour
{
    [SerializeField] private OVRInput.Button input;
    [SerializeField] private OVRInput.RawButton rawInput;
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private void Update()
    {
        if (OVRInput.GetDown(input))
            onPressed.Invoke();
        if (OVRInput.GetUp(input))
            onReleased.Invoke(); 
        
        if (OVRInput.GetDown(rawInput))
            onPressed.Invoke();
        if (OVRInput.GetUp(rawInput))
            onReleased.Invoke();
    }
}
