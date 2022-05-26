using UnityEngine;

public class WorldToScreenFollower : MonoBehaviour
{
    public Transform target = null;
    private Camera cam;

#if UNITY_EDITOR
    [SerializeField] private bool useDebug = false;
#endif
    protected virtual void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }


    private void LateUpdate()
    {
        if (cam == null)
        { 
            cam = Camera.main;
            if (cam == null)
                ShowMessage("Camera is null of " + name);
        }

        if (target == null)
            ShowMessage("Follow target is null of " + name);
        else if(target == transform.parent)
            ShowMessage("Invalid follow target " + name);
        else
            transform.position = cam.WorldToScreenPoint(target.position);

        
    }

    private void ShowMessage(string message)
    {
#if UNITY_EDITOR
        if (useDebug)
            Debug.Log("<color=cyan>" + name + " </color>");
#endif
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        LateUpdate();
    }
#endif
}
