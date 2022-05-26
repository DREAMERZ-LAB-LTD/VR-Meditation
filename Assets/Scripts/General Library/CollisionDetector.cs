using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CollisionDetector : MonoBehaviour
{
    public interface ICollisionDetector
    { 
        public void OnEnter(Transform other);
        public void OnStay(Transform other);
        public void OnExit(Transform other);
    }
    public enum InterfaceResponsMode
    {
        Self,
        Child,
    }

    [System.Flags]
    public enum DetectionMode
    { 
        OnCollisionEnter = 0x00000001,
        OnCollisionStay  = 0x00000002,
        OnCollisionExit  = 0x00000004,
        OnTriggerEnter   = 0x00000008,
        OnTriggerStay    = 0x00000100,
        OnTriggerExit    = 0x00000200,
    }
    public enum DetectionType
    { 
        Enter,
        Stay,
        Exit,
    }

    [System.Serializable]
    public class DetectionEvent
    {
        public UnityEvent OnValid;
        public UnityEvent Oninvalid;
    }

#if UNITY_EDITOR
    [Tooltip("Will never use, \n Just for track why we are using this timer"),
    SerializeField, TextArea(5, 10)]
    private string Description;
#endif


    [Header("Interface Callback Setup")]
    [SerializeField] private InterfaceResponsMode interfaceCallback = InterfaceResponsMode.Self;

    [Header("Detector Setup")]
    [SerializeField] private DetectionMode collisionMode = DetectionMode.OnCollisionEnter;
    [SerializeField] private List<string> targetTags = new List<string>();
    private List<Transform> overlappingObjects = new List<Transform>();

    [Header("Callback Event")]
    public DetectionEvent onEnter;
    public DetectionEvent onStay;
    public DetectionEvent onExit;
    public UnityEvent onOverlapBegin;
    public UnityEvent onOverlapEnd;

    private bool isValidTag(string tag)
    {
        if (targetTags.Count == 0)
            return true;

        for (int i = 0; i < targetTags.Count; i++)
        {
            if (tag == targetTags[i])
                return true;
        }
        return false;
    }
    /// <summary>
    /// called when collission detect
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnDetectedVaid(Transform other) { }
    protected virtual void OnDetectedInvaid(Transform other) { }
    private void OnDetected(DetectionMode mode, Transform detectedObj, DetectionType detectionType)
    {
        if ((collisionMode & mode) != mode || !enabled || !gameObject.activeInHierarchy) return;

        bool isValidObj = isValidTag(detectedObj.tag);
        bool interfaceWithChild = interfaceCallback == InterfaceResponsMode.Child;
     

        switch (detectionType)
        {
            case DetectionType.Enter:
                if (isValidObj)
                {
                    OnDetectedVaid(detectedObj);

                    if (overlappingObjects.Count == 0)
                        onOverlapBegin.Invoke();

                    overlappingObjects.Add(detectedObj);

                    var collissionListners = interfaceWithChild ? GetComponentsInChildren<ICollisionDetector>() :
                                                                 GetComponents<ICollisionDetector>();
                    foreach (var coll in collissionListners)
                        if (coll != null)
                            coll.OnEnter(detectedObj);

                    onEnter.OnValid.Invoke();
                }
                else
                {
                    OnDetectedInvaid(detectedObj);
                    onEnter.Oninvalid.Invoke();
                }
                break;


            case DetectionType.Stay:
                if (isValidObj)
                {
                    OnDetectedVaid(detectedObj);
                    var collissionListners = interfaceWithChild ? GetComponentsInChildren<ICollisionDetector>() :
                                                                  GetComponents<ICollisionDetector>();
                    foreach (var coll in collissionListners)
                        if (coll != null)
                            coll.OnStay(detectedObj);
                    onStay.OnValid.Invoke();
                }
                else
                {
                    OnDetectedInvaid(detectedObj);
                    onStay.Oninvalid.Invoke();
                }
                break;


            case DetectionType.Exit:
                if (isValidObj)
                {
                    overlappingObjects.Remove(detectedObj);
                }
                

                if (overlappingObjects.Count == 0 && isValidObj)
                    onOverlapEnd.Invoke();


                if (isValidObj)
                {
                    OnDetectedVaid(detectedObj);
                    var collissionListners = interfaceWithChild ? GetComponentsInChildren<ICollisionDetector>() :
                                                                  GetComponents<ICollisionDetector>();
                    foreach (var coll in collissionListners)
                        if (coll != null)
                            coll.OnExit(detectedObj);
                    onExit.OnValid.Invoke();
                }
                else
                {
                    OnDetectedInvaid(detectedObj);
                    onExit.Oninvalid.Invoke();
                }
                break;
        }
    }


    #region Collision Detection
    private void OnCollisionEnter(Collision collision) => OnDetected(DetectionMode.OnCollisionEnter, collision.transform, DetectionType.Enter);
    private void OnCollisionStay(Collision collision) => OnDetected(DetectionMode.OnCollisionStay, collision.transform, DetectionType.Stay);
    private void OnCollisionExit(Collision collision) => OnDetected(DetectionMode.OnCollisionExit, collision.transform, DetectionType.Exit);
    private void OnTriggerEnter(Collider other) => OnDetected(DetectionMode.OnTriggerEnter, other.transform, DetectionType.Enter);
    private void OnTriggerStay(Collider other) => OnDetected(DetectionMode.OnTriggerStay, other.transform, DetectionType.Stay);
    private void OnTriggerExit(Collider other) => OnDetected(DetectionMode.OnTriggerExit, other.transform, DetectionType.Exit);
    #endregion Collision Detection
}
