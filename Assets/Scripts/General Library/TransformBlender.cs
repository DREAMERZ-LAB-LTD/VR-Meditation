using UnityEngine;

[RequireComponent(typeof(Blender))]

public class TransformBlender : MonoBehaviour, Blender.IBlender
{
    [System.Flags]
    public enum BlendMode
    { 
        Transforming = 0x00000001,
        Rotating     = 0x00000010,
        Scaling      = 0x00000100
    }

    [SerializeField] private BlendMode blendMode = BlendMode.Rotating;
    [SerializeField] private Transform a, b;
 
    public void SetPointA(Transform a) { this.a = a; }
    public void SetPointB(Transform b) { this.b = b; }
   
    public void OnBegin() { }
    public void OnEnd() { }
    public void OnBlending(float t)
    {
        if ((blendMode & BlendMode.Rotating) == BlendMode.Rotating)
            transform.localEulerAngles = Vector3.Lerp(a.localEulerAngles, b.localEulerAngles, t);

        if ((blendMode & BlendMode.Transforming) == BlendMode.Transforming)
            transform.localPosition = Vector3.Lerp(a.localPosition, b.localPosition, t);

        if ((blendMode & BlendMode.Scaling) == BlendMode.Scaling)
            transform.localScale = Vector3.Lerp(a.localScale, b.localScale, t);
    }
}
