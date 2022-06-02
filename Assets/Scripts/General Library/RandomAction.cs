using UnityEngine;
using UnityEngine.Events;


namespace GeneralLibrary
{
    public class RandomAction : Action
    {
        public enum Condition
        {
            Equal,
            NotEqual,
            LessThan,
            LessThanEqual,
            GreaterThan,
            GreaterThanEqual,
        }


        [SerializeField, Range(0, 1)] private float target = 0.5f;
        [SerializeField] private Condition condition = Condition.GreaterThan;
#if UNITY_EDITOR
        [SerializeField] private bool useDebug = false;
#endif
        public delegate void ActionStatus(bool isValid);
        public ActionStatus OnRandomAction;
        public UnityEvent OnInvalid;
        /// <summary>
        /// return true if random number satisfy the given condition
        /// </summary>
        /// <param name="number">random number</param>
        /// <param name="desired"> target number to compare between</param>
        /// <returns></returns>
        private bool IsValidnumber(float number, float desired)
        {
            switch (condition)
            {
                case Condition.Equal:
                    return number == desired;
                case Condition.NotEqual:
                    return number != desired;
                case Condition.LessThan:
                    return number < desired;
                case Condition.LessThanEqual:
                    return number <= desired;
                case Condition.GreaterThan:
                    return number > desired; ;
                case Condition.GreaterThanEqual:
                    return number >= desired;
                default:
                    return false;
            }
        }


        protected override void Run()
        {
            bool valid = false;
            float number = float.NaN;
            float t = float.NaN;

            int x = 0;
            int y = 101;
            t = target * (y - x);
            number = Random.Range(x, y);
            valid = IsValidnumber(number, t);

            ShowDebug(number, t, valid);

            if (valid)
            {
                base.Run();
                if (OnRandomAction != null)
                    OnRandomAction.Invoke(true);
            }
            else
            {
                if (OnRandomAction != null)
                    OnRandomAction.Invoke(false);
                OnInvalid.Invoke();
            }
        }

        //private void Update()
        //{
        //    if (executeSource == ActionSource.OnUpdate);
        //        Run();
        //}


        private void ShowDebug(float number, float desired, bool valid)
        {
#if UNITY_EDITOR
            if (useDebug)
                Debug.Log("<color=cyan> Desired Number is " + desired + " Random Number = " + number + " is Valid = " + valid + " </color>");
#endif
        }
    }
}