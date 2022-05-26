using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    public enum RelativeSpace
    { 
        Local,
        World
    }

    [SerializeField] private RelativeSpace relativeSpace = RelativeSpace.World;


    private Vector3 velocity = new Vector3();
    private Vector3 prePosition = new Vector3();

    public Vector3 GetVelocity => velocity;
    public Vector3 GetLocalVelocity(Transform space) => space.InverseTransformPoint(space.position + velocity);

    private bool UseWorldSpace => relativeSpace == RelativeSpace.World;
    public bool isMovingSameDirection(Vector3 direction)
    {
        var selfAcceleration = transform.InverseTransformPoint(transform.position + velocity).normalized;
        var otherAcceleration = transform.InverseTransformPoint(transform.position + direction).normalized; ;

        var movingSign = Vector3.Dot(selfAcceleration, otherAcceleration);
        return movingSign > 0;
    }



    void Awake()
    {
        if(UseWorldSpace)
            prePosition = transform.position;
        else 
            prePosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        if (UseWorldSpace)
        {
            velocity = transform.position - prePosition;
            prePosition = transform.position;
        }
        else
        {
            velocity = transform.localPosition - prePosition;
            prePosition = transform.localPosition;
        }
    }
}
