using UnityEngine;
using UnityEngine.Events;

public class Action : MonoBehaviour
{
    [SerializeField, Tooltip("Event invoking source")]
    protected ExecuteAndTerminateSource executeSource = 0x00000000;

    [Header("Calback Event"), Tooltip("Invoke it self when input source thread called")]
    [SerializeField] private UnityEvent OnExecute;
    
    
    #region Unity Thread
    protected virtual void Awake() => ExecuteFrom(ExecuteAndTerminateSource.OnAwake);
    protected virtual void Start() => ExecuteFrom(ExecuteAndTerminateSource.OnStart);
    protected virtual void OnEnable() => ExecuteFrom(ExecuteAndTerminateSource.OnEnable);
    protected virtual void OnDisable() => ExecuteFrom(ExecuteAndTerminateSource.OnDisable);
    protected virtual void OnDestroy() => ExecuteFrom(ExecuteAndTerminateSource.OnDestroy);
    #endregion Unity Thread


    private void ExecuteFrom(ExecuteAndTerminateSource source)
    {
        if ((executeSource & source) != source ||  !enabled)
            return;

        Run();
    }

    protected virtual void Run() {OnExecute.Invoke();}
}