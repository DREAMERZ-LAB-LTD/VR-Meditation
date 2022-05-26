using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> panels = new List<GameObject>();


    /// <summary>
    /// Hide all of the panels.
    /// </summary>
    public virtual void HideAll()
    { 
        foreach (GameObject go in panels)
                if(go)
                    go.SetActive(false);
    }

    /// <summary>
    /// Hide all of the active panels then show only target panel
    /// </summary>
    /// <param name="index">Target panels index</param>
    public virtual void Show(int index)
    {
        if (!isValidPanel(index))
            return;
        HideAll();
        if(panels[index])
            panels[index].SetActive(true);

    }

    /// <summary>
    /// show target panel additivly with existing panels
    /// </summary>
    /// <param name="index">Target panels index</param>
    public virtual void ShowAdditive(int index)
    {
        if (!isValidPanel(index))
            return;
        if (panels[index])
            panels[index].SetActive(true);

    }


    /// <summary>
    /// return true if panels indes is valid.
    /// </summary>
    /// <param name="index">Target panels index</param>
    /// <returns></returns>
    protected bool isValidPanel(int index)
    {
        bool isValid = index < panels.Count && index >= 0;
#if UNITY_EDITOR
        if (!isValid)
        {
            Debug.Log("<color=cyan> "+ index + "</color>");
            Debug.Log("<color=cyan> Index out of bound of UI panel. Name of game object is " + name + "</color>");
        }
#endif

        return isValid;
    }
}
