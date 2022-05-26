using UnityEngine;

public class LookAt : MonoBehaviour
{
    [System.Flags] public enum Axis
    { 
        X   = 0001,
        Y   = 0010,
        Z   = 0100
    }
    
    [SerializeField] private Axis axis = Axis.X  | Axis.Y | Axis.Z;
    [SerializeField] private float speed = 1.00f;
    [SerializeField] protected Transform lookTarget;
    protected virtual void LateUpdate()
    {
        if (lookTarget)
        { 
            Vector3 lookDirection = GetLookDirection(lookTarget.position);
            transform.forward = Vector3.Lerp(transform.forward, lookDirection, speed * Time.deltaTime);
        }
    }
    /// <summary>
    /// return look direction 
    /// </summary>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    private Vector3 GetLookDirection(Vector3 targetPoint)
    { 
        if ((axis & Axis.X) != Axis.X)
            targetPoint.x = transform.position.x;

        if ((axis & Axis.Y) != Axis.Y)
            targetPoint.y = transform.position.y;
        
        if ((axis & Axis.Z) != Axis.Z)
            targetPoint.z = transform.position.z;

        Vector3 lookDir = targetPoint - transform.position;
        return lookDir.normalized;
    
    }

  
    public void ChangeLookTarget(Transform newLookTarget)
    {
        lookTarget = newLookTarget;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (lookTarget == null && enabled)
        { 
            Debug.Log("<color=cyan>Look Target field is null</color>");
        }
    }
#endif

}
