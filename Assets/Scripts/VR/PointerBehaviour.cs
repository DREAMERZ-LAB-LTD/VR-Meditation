using UnityEngine;

public class PointerBehaviour : MonoBehaviour
{
    [SerializeField] private LaserPointer.LaserBeamBehavior type = LaserPointer.LaserBeamBehavior.OnWhenHitTarget;
    private void Awake()
    {
        var laserPointer = FindObjectOfType<LaserPointer>();
        if (laserPointer)
            laserPointer.laserBeamBehavior = type;
    }
}
