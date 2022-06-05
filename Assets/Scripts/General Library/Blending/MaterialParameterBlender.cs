using UnityEngine;

namespace GeneralLibrary
{
    public class MaterialParameterBlender : MaterialParameter, Blender.IBlender
    {
        [Header("Blending Value Setup")]
        [SerializeField] private ParameterType parameterType = ParameterType.Float;
        [SerializeField] private float floatA = 0f, floatB = 1.0f;
        [SerializeField] private int intA = 0, intB = 1;
        [SerializeField] private Color colorA = Color.white, colorB = Color.green;
        [SerializeField] private Vector4 vectorA = Color.white, vectorB = Color.green;

        public void SetFloatA(float a) => this.floatA = a;
        public void SetFloatB(float b) => this.floatB = b;      
        public void SetIntA(int a) => this.intA = a;
        public void SetIntB(int b) => this.intB = b;  
        public void SetColorA(Color a) => this.colorA = a;
        public void SetColorB(Color b) => this.colorB = b;  
        public void SetVectorA(Vector4 a) => this.vectorA = a;
        public void SetVectorB(Vector4 b) => this.vectorB = b;

        public void OnBegin() { }
        public void OnEnd() { }
        public void OnBlending(float t)
        {
            switch (parameterType)
            {
                case ParameterType.Float:
                    SetFloat(Mathf.Lerp(floatA, floatB, t));
                    break;
                case ParameterType.Int:
                    Setint((int)Mathf.Lerp(intA, intB, t));
                    break;
                case ParameterType.Color:
                    SetColor(Color.Lerp(colorA, colorB, t));
                    break;
                case ParameterType.Vector:
                    SetVector(Vector4.Lerp(vectorA, vectorB, t));
                    break;
            }
        }

    }
}