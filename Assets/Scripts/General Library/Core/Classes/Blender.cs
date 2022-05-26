using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Blender : MonoBehaviour
{
    public interface IBlender
    {
        void OnBegin();
        void OnEnd();
        void OnBlending(float t);
    }
    public enum LoopType
    {
        None,
        Both,
        Clockwise,
        AntiClockwise
    }

    [Header("Blender Setup")]
    [SerializeField] private ExecuteSource executeSource = 0x00;
    [SerializeField] private LoopType loopType = LoopType.None;
    [SerializeField] private float speed = 1;
    private IBlender[] rensponses = null;

    [Header("Callback Events")]
    public UnityEvent OnBlendBegin;
    public UnityEvent OnBlendEnd;

    #region Unity Message
    private void Awake()
    { 
         rensponses = GetComponents<IBlender>();
        if (ExecuteSource.OnAwake == (executeSource & ExecuteSource.OnAwake))
            StartBlending();
    }

    private void Start()
    {
        if (ExecuteSource.OnStart == (executeSource & ExecuteSource.OnStart))
            StartBlending();
    }
    private void OnEnable()
    {
        if (ExecuteSource.OnEnable == (executeSource & ExecuteSource.OnEnable))
            StartBlending();
    }

    private void OnDisable() => StopAllCoroutines();
    

    #endregion Unity Message

    #region Blending
    public void StartBlending()
    {
        StopAllCoroutines();
        StartCoroutine(BlendingRoutine());
    }
    private void UpdateLoop(ref float t)
    {
        bool isComplete = false;
        switch (loopType)
        {
            case LoopType.None:
                isComplete = t > 1 || t < 0;
                if (isComplete)
                    StopAllCoroutines();
                break;

            case LoopType.Both:
                isComplete = t >= 1 || t <= 0;
                if (isComplete)
                    speed = -speed;
                break;

            case LoopType.Clockwise:
                speed = Mathf.Abs(speed);
                isComplete = t >= 1;
                if (isComplete)
                    t = 0;
                break;

            case LoopType.AntiClockwise:
                speed = -Mathf.Abs(speed);
                isComplete = t <= 0;
                if (isComplete)
                    t = 1;
                break;
        }

        if (isComplete)
        {
            if (rensponses != null)
                foreach (var response in rensponses)
                    response.OnEnd();
            OnBlendEnd.Invoke();
        }
    }

    private IEnumerator BlendingRoutine()
    {
        if (rensponses != null)
            foreach (var response in rensponses)
                response.OnBegin();

        float t = 0;
        OnBlendBegin.Invoke();
        while (true)
        {
            t += Time.fixedDeltaTime * speed;
            if (rensponses != null)
                foreach (var response in rensponses)
                    response.OnBlending(t);

            UpdateLoop(ref t);

            yield return null;
        }
    }
    #endregion Blending
}
