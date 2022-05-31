using UnityEngine;
using UnityEngine.Events;

public class HierarchyTreeController : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    [SerializeField] private UnityEvent OnExecute;
    private void Awake()
    {
        if (target == null)
            target = transform;
    }
    public void DeattachChildren()
    {
        target.DetachChildren();
        OnExecute.Invoke();
    }

    public void SetParent()
    {
        transform.SetParent(target);
        OnExecute.Invoke();
    }

}
