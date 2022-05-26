using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchGesture : MonoBehaviour
{
    [Flags]    
    public enum InputType
    { 
        SingleTap       = 0001,
        FingerDrag      = 0010,
        MouseHold       = 0100
    }

    [Flags]
    public enum FilterMask
    { 
        ALL_UI              = 0x00000001,
        SelfGameObject      = 0x00000020,
    }


    [SerializeField, Header("User Input Type To Start")] 
    private InputType inputType = InputType.SingleTap;
    [SerializeField]private FilterMask filterMask = 0x00000000;

    [SerializeField, Header("Callback Event")] 
    private UnityEvent OnTapBegin;
    private UnityEvent OnGestureFailed;
    
    private Vector3 clickPoint = new Vector3();
    private float clickTime = 0;
    private bool gestureCanceled = true;

    protected virtual void Update()
    {
        VerifyGesture();
        if (GetUserInput() && !gestureCanceled)
        {
            OnValidInput();
        }
    }

    protected virtual void OnValidInput()
    {
        OnTapBegin.Invoke();
    }

    private bool GetUserInput()
    {
        bool isInputDetected = false;
        if ((inputType & InputType.SingleTap) == InputType.SingleTap)//
        {
            if (Input.GetMouseButtonDown(0))
                clickPoint = Input.mousePosition;
            if (Input.GetMouseButtonUp(0) && !isInputDetected)
            {
                Vector3 direction = Input.mousePosition - clickPoint;
                isInputDetected = direction.magnitude < 5;
            }
        }

        if ((inputType & InputType.FingerDrag) == InputType.FingerDrag)
        {
            if (Input.GetMouseButtonDown(0))
                clickPoint = Input.mousePosition;
            if (Input.GetMouseButton(0) && !isInputDetected)
            {
                Vector3 direction = Input.mousePosition - clickPoint;
                isInputDetected = direction.magnitude > 5;
            }
        }

        if ((inputType & InputType.MouseHold) == InputType.MouseHold)
        {
            if (Input.GetMouseButtonDown(0))
                clickTime = Time.time;
            if (Input.GetMouseButton(0) && !isInputDetected)
                isInputDetected = Time.time > clickTime + 1;
        }

        return isInputDetected;
    }

    /// <summary>
    /// Verify the gesture input with fiven filter mask
    /// </summary>
    private void VerifyGesture()
    {
        if (filterMask == 0x00000000)// No masking and return to leave rest of the Filtering process
        {
            if(gestureCanceled)
                gestureCanceled = false;
            return;
        }

        if (Input.GetMouseButtonDown(0) )
                gestureCanceled = false;

      
        if ((filterMask & FilterMask.ALL_UI) == FilterMask.ALL_UI) //Detecting pointer is over ui or outside
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                if (!gestureCanceled)
                    gestureCanceled = EventSystem.current.IsPointerOverGameObject();
            }
        }

        if ((filterMask & FilterMask.SelfGameObject) == FilterMask.SelfGameObject) //Detecting pointer is over the self object that contain the script in inspector
        {
            if(!gestureCanceled)
                gestureCanceled = PointerOverSelfObject();
        }
    }

    /// <summary>
    /// return true if gesture detect pointer is over the self object that contain this script in the inspector
    /// </summary>
    /// <returns></returns>
    public  bool PointerOverSelfObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (var o in results)
        {
            if (transform == o.gameObject.transform)
                return true;
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i) == o.gameObject.transform)
                    return true;
        }
            
        return false;
    }
}
