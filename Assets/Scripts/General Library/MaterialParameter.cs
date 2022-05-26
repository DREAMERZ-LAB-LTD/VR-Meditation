using UnityEngine;

public class MaterialParameter : Debuggable
{
    #region Macros
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
    #endregion Macros


    [Header("Parameter Setup")]
    [SerializeField] private MaterialSource materialSource = MaterialSource.FromRenderer;
    [SerializeField] protected string parameter = string.Empty;
    [SerializeField] protected Material[] materials = null;

    #region Virtual Methods
    protected virtual void SetFloat(float value)
    {
        if (materials != null)
        { 
            foreach (var mat in materials)
                mat.SetFloat(parameter, value);

            ShowMessage("Float value = '"+ value + "' set successfull into the parameter '"+ parameter + "' of this materials ");
        }
    }
    protected virtual void Setint(int value)
    {
        if (materials != null)
        { 
            foreach (var mat in materials)
                mat.SetInt(parameter, value);

            ShowMessage("Int value = '" + value + "' set successfull into the parameter '" + parameter + "' of this materials ");
        }
    }
    protected virtual void SetColor(Color value)
    {
        if (materials != null)
        { 
            foreach (var mat in materials)
                mat.SetColor(parameter, value);

            ShowMessage("Color value = '" + value + "' set successfull into the parameter '" + parameter + "' of this materials ");
        }
    }

    protected virtual void SetVector(Vector4 value)
    {
        if (materials != null)
        { 
            foreach (var mat in materials)
                mat.SetVector(parameter, value);
        
            ShowMessage("Vector value = '" + value + "' set successfull into the parameter '" + parameter + "' of this materials ");
        }

    }
    #endregion Virtual Methods

    #region Debugging
#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        if (materialSource == MaterialSource.FromRenderer)
        {
            var rend = GetComponent<Renderer>();
            if (rend)
                materials = rend.materials;
            else
                if (useDebug)
                ShowMessage("No renderer found to access material property");
        }

        if (useDebug)
            if (materials.Length == 0)
                ShowMessage("No material assigned");
    }
#endif
    #endregion Debugging
}
