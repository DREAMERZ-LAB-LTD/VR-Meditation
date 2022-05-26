using UnityEngine;

public class Follower : MonoBehaviour
{
    [System.Flags]
    public enum Axis
    {
        X = 0x01,
        Y = 0x10,
        Z = 0100
    }

    [Header("Follow Setup")]

    [SerializeField, Tooltip("Follow Target Object")]
    private Transform target = null;
    [SerializeField, Tooltip("Follow Axis")]
    private Axis axis = Axis.X | Axis.Y| Axis.Z;
    [SerializeField, Tooltip("Follow Speed")] private float speed = 1;

    private Vector3 offset = new Vector3(); //follow speed

#if UNITY_EDITOR
    [Header("Debug Setup")]
    [SerializeField] private bool showDebug = false;
#endif


    private void Awake()
    {
        if (target)
            offset = target.position - transform.position;
#if UNITY_EDITOR
        if (target == null && showDebug)
            Debug.Log("<color=cyan> Follower target is null </color>");
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        if (target == null && showDebug)
            Debug.Log("<color=cyan> Follower target is null </color>");
#endif

        if(target)
            transform.transform.position = Vector3.MoveTowards(transform.position, GetPoint(), speed * Time.deltaTime);
    }

    /// <summary>
    /// return target target follow position
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPoint()
    {
        var point = target.position - offset;

        if((axis & Axis.X) != Axis.X)
            point.x = transform.position.x;

        if ((axis & Axis.Y) != Axis.Y)
            point.y = transform.position.y;

        if ((axis & Axis.Z) != Axis.Z)
            point.z = transform.position.z;

        return point;
    }

}
