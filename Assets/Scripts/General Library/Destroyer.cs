using UnityEngine;
using UnityEngine.Events;


namespace GeneralLibrary
{
    public class Destroyer : Action
    {
        [Header("Destroyer Setup")]
        [SerializeField, Tooltip("How many time it will exist in the scene.\n if value is negetive it object will never destroy")]
        private float destroyDelay = 1.00f;

#pragma warning disable 649
        [SerializeField] private UnityEvent OnDestroying;
#pragma warning restore 649


        /// <summary>
        ///  It will start execute from unity thread that you choices from inspector
        /// </summary>
        /// <param name="source"></param>
        protected override void Run()
        {
            MakeItDestroy();
        }

        /// <summary>
        /// This object will destroy based on life time.
        /// That kind of life time initialized from inspector.
        /// </summary>
        public void MakeItDestroy() => Destroy(gameObject, destroyDelay);

        /// <summary>
        ///  This object will destroy based on delay time.
        /// </summary>
        /// <param name="delayTime">Destroy delay</param>
        public void MakeItDestroy(float delayTime) => Destroy(gameObject, delayTime);

        protected override void OnDestroy() => OnDestroying.Invoke();


    }
}