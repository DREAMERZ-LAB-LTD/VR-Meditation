using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    [System.Flags] private enum ExecuteSource
    {
        OnAwake = 0x01,
        OnEnable = 0010,
        OnStart = 0100,
    }
    private enum UpdateMode
    {
        Discreate,
        InASecond,
        Continuous
    }

    #region Member Variables
#if UNITY_EDITOR
    [Tooltip("Will never use, \n Just for track why we are using this timer"),
    SerializeField, TextArea(5, 10)]
    private string Description;
#endif

    [Header("Timer Setup")]
    [Tooltip("Counter target time in seconds")]
    public float targetTime = 0;
    [SerializeField, Tooltip("If true timer will start on Awake")]
    private ExecuteSource executeSource = ExecuteSource.OnAwake;
    [SerializeField, Tooltip("Timer will updating each of the delta seconds otherwise updating after complete each of the second")]
    private UpdateMode updateMode = UpdateMode.Discreate;
    private bool isPaused = false;

    [Header("ValueRender Setup On UI")]
    [SerializeField, Tooltip("If true timer will show how much time left \n otherwise will show continus increase number")]
    bool showLeftTime = false;
    [SerializeField, Tooltip("message will print before the time value on the UI screen")]
    private string preMessage = string.Empty;
    [SerializeField, Tooltip("message will print after the time value on the UI screen")]
    private string postMessage = string.Empty;
    [SerializeField, Tooltip("Where timer value will going to render, \n it could be Slider, Image, Text & TextMeshProUGUI")]
    private Graphic statusRender = null;
    private Slider statusSlider = null;
    private Image statusImage = null;
    private Text statusTxt = null;
    private TextMeshProUGUI statusTMP = null;


    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnTimeStart;
    [SerializeField] private UnityEvent OnTimeOver;
    [SerializeField] private UnityEvent OnTimerUpdating;
    [SerializeField] private UnityEvent OnTimerStop;

    private Coroutine activeTimer = null;
    private float time = 0;
    #endregion  Member Variables

    #region Initialize & status
    private void OnInitialize()
    {
        if (statusRender)
        {
            var renderType = statusRender.GetType();

            if (renderType == typeof(Text) && statusTxt == null)
                statusTxt = statusRender.GetComponent<Text>();
            if (renderType == typeof(TextMeshProUGUI) && statusTMP == null)
                statusTMP = statusRender.GetComponent<TextMeshProUGUI>();
            if (renderType == typeof(Slider) && statusSlider == null)
                statusSlider = statusRender.GetComponent<Slider>();
            if (renderType == typeof(Image) && statusImage == null)
                statusImage = statusRender.GetComponent<Image>();
            if (statusImage)
                statusImage.type = Image.Type.Filled;
        }
    }

    /// <summary>
    /// preapering time to show forward or inverse
    /// </summary>
    /// <param name="currentTime">Timer current time</param>
    private void UpdateStatus(float currentTime)
    {
        if (statusRender == null)
            return;

        if (statusSlider)
        {
            statusSlider.maxValue = targetTime;
            statusSlider.value = currentTime;
        }
        if (statusImage)
        {
            statusImage.fillAmount = currentTime / targetTime;
        }


        int time = Mathf.CeilToInt(currentTime);
        int target = Mathf.CeilToInt(targetTime);
        if (showLeftTime)
            time = target - time;


        string message = preMessage + time + postMessage;
        if (statusTMP)
            statusTMP.text = message;
        if (statusTxt)
            statusTxt.text = message;
    }

    #endregion Initialize & status

    #region Unity Message
    private void OnEnable()
    {
        OnInitialize();
        if ((executeSource & ExecuteSource.OnEnable) == executeSource)
            StartTimer();
    }
    private void Awake()
    {
        OnInitialize();
        if ((executeSource & ExecuteSource.OnAwake) == executeSource)
            StartTimer();
    }

    private void Start()
    {
        if ((executeSource & ExecuteSource.OnStart) == executeSource)
            StartTimer();
    }
    #endregion UnityMessage

    #region Timer Control
    public void StartTimer()
    {
        if (!gameObject.activeInHierarchy || !enabled)
            return;

        time = 0;
        StopTimer();
        OnTimeStart.Invoke();
        activeTimer = StartCoroutine(Counter());
    }

    public void StopTimer()
    {
        if (activeTimer != null)
        { 
            StopCoroutine(activeTimer);
            OnTimerStop.Invoke();
        }
    }
    #endregion Timer Control

    #region Timer Routine
    /// <summary>
    /// actual time counter thread
    /// </summary>
    /// <returns></returns>
    private IEnumerator Counter()
    {
        bool complete = time >= targetTime;
        float timeOffset = Time.time;
        while (!complete)
        {
            while (isPaused)
                yield return null;


            switch (updateMode)
            {

                case UpdateMode.Discreate:
                    yield return new WaitForSeconds(targetTime);
                    time = targetTime;
                    complete = true;
                    break;

                case UpdateMode.InASecond:
                    time++;
                    complete = time > targetTime - 1;
                    yield return new WaitForSeconds(1);
                    break;

                case UpdateMode.Continuous:
                    time = Time.time - timeOffset;
                    complete = time >= targetTime -1 ;
                    yield return null;
                    break;
            }

            UpdateStatus(time);
            OnTimerUpdating.Invoke();
        }
        OnTimeOver.Invoke();
    }
    #endregion  Timer Routine
}