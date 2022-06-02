using UnityEngine;

namespace GeneralLibrary
{
    public class Debuggable : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Debug Setup")]
        [SerializeField] protected bool useDebug = false;
#endif

        protected void ShowMessage(string message)
        {
#if UNITY_EDITOR
            if (useDebug)
                Debug.Log("<color=cyan>" + message + " </color>");
#endif
        }
    }
}