using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace GeneralLibrary
{
    public class SlowMotion : MonoBehaviour
    {
        [System.Flags]
        public enum Type
        {
            None = 0,
            Creator = 1,//slowmotion creator
            Reseter = 2,//slowmotion reseter
        }
        public enum MotionDirection
        {
            Standard = 0x00,
            Negetive = 0x01,
            Positive = 0x10,
        }

        [SerializeField] private Type type = Type.Creator;
        [Header("Maker Setup")]
        [SerializeField, Range(0, 100)] private float probability = 100;
        [SerializeField] protected Vector2 targetMotionRate = new Vector2(0.3f, .5f);
        [SerializeField] private float effectDuration = 0.1f;

        [Header("Reseter Setup")]
        [SerializeField] private float resetSpeed = 1;

        [Header("Callback Events")]
        [SerializeField] private UnityEvent OnChanged;
        [SerializeField] private UnityEvent OnRested;

        private Coroutine reseter;
        private static float motionStartTime = 0;
        private static MotionDirection motionDirection = MotionDirection.Standard;


        private bool isValidProbability()
        {
            int probability = Random.Range(0, 100);
            if (probability <= this.probability)
                return true;
            else
                return false;
        }

        /// <summary>
        ///  Set the target motion value as a Time.timeScale randomly in range of "targetMotionRate" field of inspector
        /// </summary>
        public virtual void ApplyMotion()
        {
            float motionRate = Random.Range(targetMotionRate.x, targetMotionRate.y);
            ApplyMotion(motionRate);
        }

        /// <summary>
        /// Set the target motion value as a Time.timeScale
        /// </summary>
        /// <param name="motionRate">New TimeScale value</param>
        public virtual void ApplyMotion(float motionRate)
        {
            if (isValidProbability() && Type.Creator == (Type.Creator & type))
            {
                if (motionRate > 1)
                {
                    motionDirection = MotionDirection.Positive;
                    OnChanged.Invoke();
                }
                else if (motionRate < 1)
                {
                    motionDirection = MotionDirection.Negetive;
                    OnChanged.Invoke();
                }
                else if (motionRate == 1)
                {
                    motionDirection = MotionDirection.Standard;
                    OnRested.Invoke();
                }

                motionStartTime = Time.time;
                Time.timeScale = motionRate;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }

        /// <summary>
        /// reseting Time.timeScale value to reach 1 
        /// reseting Time.fixedDeltaTime to reach 0.02
        /// </summary>
        private void ResetSlowMotion()
        {
            bool isValidResetTime = motionStartTime + effectDuration * Time.timeScale < Time.time;

            if (motionDirection != MotionDirection.Standard && isValidResetTime)
            {
                float deltaSpeed = resetSpeed * Time.deltaTime;
                if (motionDirection == MotionDirection.Positive)
                    deltaSpeed *= -1;


                float newTimeScale = Time.timeScale + deltaSpeed;
                Time.timeScale = Mathf.Clamp(newTimeScale, 0, 1);
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                if (Time.timeScale == 1)
                {
                    motionDirection = MotionDirection.Standard;
                    OnRested.Invoke();
                }
            }
        }
        /// <summary>
        /// if TimeScale not equal 1 then it will try to update timescale continusly to make standard
        /// Value will updating to (Time.timeScale = 1 and Time.fixedDeltaTime = 0.02)
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetMotion()
        {
            while (true)
            {
                ResetSlowMotion();
                yield return null;
            }
        }

        /// <summary>
        /// reseting the slowmotion effect asynchronously
        /// </summary>
        public virtual void ResetMotionAsync()
        {
            if (reseter != null)
                StopCoroutine(reseter);
            reseter = StartCoroutine(ResetMotion());
        }

        protected virtual void Awake()
        {
            if (Type.Reseter == (Type.Reseter & type))
            {
                ResetMotionAsync();
            }
        }
    }
}