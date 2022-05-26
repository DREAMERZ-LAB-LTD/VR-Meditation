using UnityEngine;

public class MaterialParameter : MonoBehaviour
{
    public enum ParameterType
    { 
        Float,
        Int,
        Color,
        Vector
    }

    public enum MaterialSource
    { 
        FromRenderer,
        FromAssets
    }

    [Header("Parameter Setup")]
    [SerializeField] private MaterialSource materialSource = MaterialSource.FromRenderer;
    [SerializeField] protected string parameter = string.Empty;
    [SerializeField] protected Material[] materials = null;

    protected virtual void SetFloat(float value)
    {
        if (materials != null)
            foreach (var mat in materials)
                mat.SetFloat(parameter, value);
    }
    protected virtual void Setint(int value)
    {
        if (materials != null)
            foreach (var mat in materials)
                mat.SetInt(parameter, value);
    }
    protected virtual void SetColor(Color value)
    {
        if (materials != null)
            foreach (var mat in materials)
                mat.SetColor(parameter, value);
    }

    protected virtual void SetVector(Vector4 value)
    {
        if (materials != null)
            foreach (var mat in materials)
                mat.SetVector(parameter, value);
    }


#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        if (materialSource == MaterialSource.FromRenderer)
        {
            var rend = GetComponent<Renderer>();
            if (rend)
                materials = rend.materials;
            else
                Debug.Log("<color=cyan> No renderer found to access material property </color>");
        }
    }
#endif
}
