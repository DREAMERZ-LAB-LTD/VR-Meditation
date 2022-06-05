using UnityEngine;
using UnityEngine.Events;

public class OVR_InputAction : MonoBehaviour
{
    public OVRInput.Controller Thumbstick;

    void Update()
    {
        if (Thumbstick == OVRInput.Controller.Touch)
            Debug.Log("Valid Input ");
        else
            Debug.Log("IN Valid Input ");
        
    }
}
