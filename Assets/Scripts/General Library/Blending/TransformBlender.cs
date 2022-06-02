using UnityEngine;

namespace GeneralLibrary
{
    public class TransformBlender : Blender
    {
        [System.Flags]
        private enum BlendMode
        {
            Transforming = 0x01,
            Rotating = 0x10,
            Scaling = 0100
        }

        [Header("Blend Parameter Setup")]
        [SerializeField] private BlendMode blendMode = BlendMode.Rotating;
        [SerializeField] private Transform a, b;

        public void SetPointA(Transform a) { this.a = a; }
        public void SetPointB(Transform b) { this.b = b; }

        protected override void OnBlending(float t)
        {
            if (a == null || b == null)
            {

                return;
            }

            if ((blendMode & BlendMode.Rotating) == BlendMode.Rotating)
                transform.localEulerAngles = Vector3.Lerp(a.localEulerAngles, b.localEulerAngles, t);

            if ((blendMode & BlendMode.Transforming) == BlendMode.Transforming)
                transform.localPosition = Vector3.Lerp(a.localPosition, b.localPosition, t);

            if ((blendMode & BlendMode.Scaling) == BlendMode.Scaling)
                transform.localScale = Vector3.Lerp(a.localScale, b.localScale, t);
        }
    }
}