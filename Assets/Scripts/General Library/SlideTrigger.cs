using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GeneralLibrary
{
    [RequireComponent(typeof(Slider))]
    public class SlideTrigger : MonoBehaviour
    {
        [Header("Movement Points")]
        [SerializeField, Range(0.00f, 1.00f)] private float min = 0.3f;
        [SerializeField, Range(0.00f, 1.00f)] private float max = 0.6f;
        [SerializeField, Range(0.00f, 5f)] private float speed = 0.5f;

        private float t = 0.00f;
        private int sign = 1;
        private bool isValidPoint(float t) => t >= min && t <= max;

        private Slider Slider = null;
        private Image sliderHandle = null;

        public KeyCode inputKey = KeyCode.Mouse0;
        [Header("Callback Events")]
        public UnityEvent OnValidTrigger;
        public UnityEvent OnInvalidTrigger;

        private void Start()
        {
            Slider = GetComponent<Slider>();
            if (Slider)
            {
                Slider.minValue = 0;
                Slider.maxValue = 1;
                sliderHandle = Slider.handleRect.GetComponent<Image>();
            }
        }

        private void UpdateValue()
        {
            if (t <= 0)
                sign = 1;
            else if (t >= 1)
                sign = -1;

            t += speed * sign * Time.deltaTime;
            t = Mathf.Clamp(t, 0, 1);

            if (Slider)
            {
                Slider.value = t;
                sliderHandle.color = isValidPoint(t) ? Color.green : Color.white;
            }
        }

        public void OnclickExecute()
        {
            if (isValidPoint(t))
                OnValidTrigger.Invoke();
            else
                OnInvalidTrigger.Invoke();
        }
        private void Update()
        {
            UpdateValue();

            if (Input.GetKeyDown(inputKey))
                OnclickExecute();
        }
    }
}