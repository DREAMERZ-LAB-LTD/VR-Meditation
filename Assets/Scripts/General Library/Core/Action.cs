using UnityEngine;
using UnityEngine.Events;


namespace GeneralLibrary
{
    public class Action : MonoBehaviour
    {
        [System.Flags]
        protected enum ExecuteSource
        {
            OnAwake = 0x00000001,
            OnStart = 0x00000010,
            OnEnable = 0x00000100,
            OnDisable = 0x00001000,
            OnDestroy = 0x00010000,
        }

        [SerializeField, Tooltip("Event invoking source")]
        protected ExecuteSource executeSource = 0x00000000;

        [Header("Calback Event"), Tooltip("Invoke it self when input source thread called")]
        [SerializeField] private UnityEvent OnExecute;


        #region Unity Thread
        protected virtual void Awake() => ExecuteFrom(ExecuteSource.OnAwake);
        protected virtual void Start() => ExecuteFrom(ExecuteSource.OnStart);
        protected virtual void OnEnable() => ExecuteFrom(ExecuteSource.OnEnable);
        protected virtual void OnDisable() => ExecuteFrom(ExecuteSource.OnDisable);
        protected virtual void OnDestroy() => ExecuteFrom(ExecuteSource.OnDestroy);
        #endregion Unity Thread


        private void ExecuteFrom(ExecuteSource source)
        {
            if ((executeSource & source) != source || !enabled)
                return;

            Run();
        }

        protected virtual void Run() { OnExecute.Invoke(); }
    }
}