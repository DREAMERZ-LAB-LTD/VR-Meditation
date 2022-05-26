using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class CollisionDetector : Debuggable
{
    #region Macros
    public interface ICollisionDetector
    { 
        public void OnEnter(Transform other);
        public void OnStay(Transform other);
        public void OnExit(Transform other);

        public void OnOverlapping(in List<Transform> overlappingObjects);
    }


    [System.Flags] private enum InterfaceResponse
    {
        Self = 0x01,
        Parent = 0010,
        Child = 0100,
    }

    [System.Flags] private enum DetectionMode
    { 
        OnCollisionEnter = 0x00000001,
        OnCollisionStay  = 0x00000002,
        OnCollisionExit  = 0x00000004,
        OnTriggerEnter   = 0x00000008,
        OnTriggerStay    = 0x00000100,
        OnTriggerExit    = 0x00000200,
    }
    private enum DetectionType
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
    #endregion Macros

    #region Propertys

#if UNITY_EDITOR
    [Tooltip("Will never use, \n Just for track why we are using this timer"),
    SerializeField, TextArea(5, 10)]
    private string Description;
#endif

    [Header("Intersect Masking Setup")]
    [SerializeField] private DetectionMode collisionMode = DetectionMode.OnCollisionEnter;
    [SerializeField] private List<string> targetTags = new List<string>();
    private List<Transform> overlappingObjects = new List<Transform>();

    [Header("Interface Response")]
    [SerializeField] private InterfaceResponse interfaceCallback = 0x00;
    private List<ICollisionDetector> interfaces = new List<ICollisionDetector>();

    [Header("Intersect Callback Events")]
    public DetectionEvent onEnter;
    public DetectionEvent onStay;
    public DetectionEvent onExit;

    [Header("Overlap Callback Events")]
    public UnityEvent onOverlapBegin;
    public UnityEvent onOverlapEnd;

    #endregion Propertys

    #region Initialize
    private void Awake()
    {
        if (InterfaceResponse.Self == (interfaceCallback & InterfaceResponse.Self))
        { 
                var selfInterfaces = GetComponents<ICollisionDetector>();
                if (selfInterfaces.Length > 0)
                    interfaces.AddRange(selfInterfaces);

            ShowMessage("Self interface respone count + " + selfInterfaces.Length);
        }
        if (InterfaceResponse.Parent == (interfaceCallback & InterfaceResponse.Parent))
        {   
            if (transform.parent)
            { 
                var parentInterfaces = transform.parent.GetComponents<ICollisionDetector>();
                if (parentInterfaces.Length > 0)
                    interfaces.AddRange(parentInterfaces);

                ShowMessage("Parent interface respone count + " + parentInterfaces.Length);
            }
        }
        if (InterfaceResponse.Child == (interfaceCallback & InterfaceResponse.Child))
        {
            List<ICollisionDetector> chidInterfaces = new List<ICollisionDetector>();
           
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child)
                { 
                    var cInterfaces = child.GetComponents<ICollisionDetector>();
                    if (cInterfaces.Length > 0)
                        chidInterfaces.AddRange(cInterfaces);
                }
            }

            if (chidInterfaces.Count > 0)
                interfaces.AddRange(chidInterfaces);
            
            ShowMessage("Child interface respone count + " + chidInterfaces.Count);
        }
    }
    #endregion Initialize

    #region Virtual Method
    protected virtual void OnDetectedVaid(Transform other) => ShowMessage("'OnDetectedVaid' method called, Intersect with : " + other.name);
    protected virtual void OnDetectedInvaid(Transform other) => ShowMessage("'OnDetectedInvaid' method called, Intersect with : " + other.name);
    protected virtual void OnOverlapping(List<Transform> overlappingObjects) => ShowMessage("'OnOverlapping' method called, Overlapping object count = " + overlappingObjects.Count);
    
    #endregion Virtual Method

    #region Collision Masking & Callback
    private bool IsValidTag(string tag)
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

    private void OnIntersectionDetected(DetectionMode mode, Transform detectedObj, DetectionType detectionType)
    {
        if ((collisionMode & mode) != mode || !enabled || !gameObject.activeInHierarchy) return;
        bool isValidObj = IsValidTag(detectedObj.tag);

        ShowMessage("Intersect with Valid Object = " + isValidObj);

        if (isValidObj)
        {
            switch (detectionType)
            {
                case DetectionType.Enter:
                    OnDetectedVaid(detectedObj);

                    if (overlappingObjects.Count == 0)
                        onOverlapBegin.Invoke();

                    if (!overlappingObjects.Contains(detectedObj))
                        overlappingObjects.Add(detectedObj);

                    var overlapCount = overlappingObjects.Count;
                    if (overlapCount > 0)
                        OnOverlapping(overlappingObjects);

                    foreach (var intfce in interfaces)
                        if (intfce != null)
                        {
                            intfce.OnEnter(detectedObj);
                            if (overlapCount > 0)
                                intfce.OnOverlapping(in overlappingObjects);
                        }

                    onEnter.OnValid.Invoke();
                    break;

                case DetectionType.Stay:
                    OnDetectedVaid(detectedObj);

                    foreach (var intfce in interfaces)
                        if (intfce != null)
                            intfce.OnStay(detectedObj);
                    onStay.OnValid.Invoke();
                    break;


                case DetectionType.Exit:

                    overlappingObjects.Remove(detectedObj);

                    OnOverlapping(overlappingObjects);

                    foreach (var intfce in interfaces)
                        if (intfce != null && overlappingObjects.Count > 0)
                            intfce.OnOverlapping(in overlappingObjects);

                    if (overlappingObjects.Count == 0)
                        onOverlapEnd.Invoke();


                    OnDetectedVaid(detectedObj);
                    foreach (var intfce in interfaces)
                        if (intfce != null)
                            intfce.OnExit(detectedObj);
                    onExit.OnValid.Invoke();
                    break;
            }
        }
        else
        {
            switch (detectionType)
            {
                case DetectionType.Enter:
                    OnDetectedInvaid(detectedObj);
                    onEnter.Oninvalid.Invoke();
                    break;

                case DetectionType.Stay:
                    OnDetectedInvaid(detectedObj);
                    onStay.Oninvalid.Invoke();
                    break;

                case DetectionType.Exit:
                    OnDetectedInvaid(detectedObj);
                    onExit.Oninvalid.Invoke();
                    break;
            }
        }
    }
    #endregion Collision Masking & Callback

    #region Collision Detection
    private void OnCollisionEnter(Collision collision) => OnIntersectionDetected(DetectionMode.OnCollisionEnter, collision.transform, DetectionType.Enter);
    private void OnCollisionStay(Collision collision) => OnIntersectionDetected(DetectionMode.OnCollisionStay, collision.transform, DetectionType.Stay);
    private void OnCollisionExit(Collision collision) => OnIntersectionDetected(DetectionMode.OnCollisionExit, collision.transform, DetectionType.Exit);
    private void OnTriggerEnter(Collider other) => OnIntersectionDetected(DetectionMode.OnTriggerEnter, other.transform, DetectionType.Enter);
    private void OnTriggerStay(Collider other) => OnIntersectionDetected(DetectionMode.OnTriggerStay, other.transform, DetectionType.Stay);
    private void OnTriggerExit(Collider other) => OnIntersectionDetected(DetectionMode.OnTriggerExit, other.transform, DetectionType.Exit);
    #endregion Collision Detection
}
