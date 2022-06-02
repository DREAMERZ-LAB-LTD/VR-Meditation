using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace GeneralLibrary
{
    public class Blender : Debuggable
    {
        #region Macros
        public interface IBlender
        {
            void OnBegin();
            void OnEnd();
            void OnBlending(float t);
        }
        [System.Flags]
        private enum ExecuteSource
        {
            OnAwake = 0x01,
            OnEnable = 0010,
            OnStart = 0100,
        }

        private enum LoopType
        {
            None,
            Both,
            Clockwise,
            AntiClockwise
        }

        [System.Flags]
        private enum InterfaceResponse
        {
            Self = 0x01,
            Parent = 0010,
            Child = 0100,
        }
        #endregion Macros

        #region Propertys
        [Header("Blender Setup")]
        [SerializeField] private ExecuteSource executeSource = ExecuteSource.OnStart;
        [SerializeField] private LoopType loopType = LoopType.None;
        [SerializeField] private InterfaceResponse interfaceResponse = 0x00;
        [SerializeField] private float speed = 1;
        private List<IBlender> interfaces = new List<IBlender>();

        [Header("Callback Events")]
        public UnityEvent m_OnBlendBegin;
        public UnityEvent m_OnBlendEnd;
        #endregion Propertys

        #region Unity Message
        private void Awake()
        {
            Initialize();
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

        #region Custom Method
        private void Initialize()
        {
            if (InterfaceResponse.Self == (interfaceResponse & InterfaceResponse.Self))
            {
                var selfInterfaces = GetComponents<IBlender>();
                if (selfInterfaces.Length > 0)
                    interfaces.AddRange(selfInterfaces);

                ShowMessage("Self interface respone count + " + selfInterfaces.Length);
            }

            if (InterfaceResponse.Parent == (interfaceResponse & InterfaceResponse.Parent))
            {
                if (transform.parent)
                {
                    var parentInterfaces = transform.parent.GetComponents<IBlender>();
                    if (parentInterfaces.Length > 0)
                        interfaces.AddRange(parentInterfaces);

                    ShowMessage("Parent interface respone count + " + parentInterfaces.Length);
                }
            }
            if (InterfaceResponse.Child == (interfaceResponse & InterfaceResponse.Child))
            {
                List<IBlender> chidInterfaces = new List<IBlender>();

                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    if (child)
                    {
                        var cInterfaces = child.GetComponents<IBlender>();
                        if (cInterfaces.Length > 0)
                            chidInterfaces.AddRange(cInterfaces);
                    }
                }

                if (chidInterfaces.Count > 0)
                    interfaces.AddRange(chidInterfaces);

                ShowMessage("Child interface respone count + " + chidInterfaces.Count);
            }

            ShowMessage("Total interface respone count + " + interfaces.Count);
        }

        public void StartBlending()
        {
            StopAllCoroutines();
            StartCoroutine(BlendingRoutine());
        }

        protected virtual void OnBlendBegin() { }
        protected virtual void OnBlendEnd() { }
        protected virtual void OnBlending(float t) { }
        #endregion Custom Method

        #region Blending Mechanics
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
                OnBlendEnd();

                if (interfaces != null)
                    foreach (var response in interfaces)
                        response.OnEnd();

                m_OnBlendEnd.Invoke();
            }
        }

        private IEnumerator BlendingRoutine()
        {
            OnBlendBegin();

            if (interfaces != null)
                foreach (var response in interfaces)
                    response.OnBegin();

            m_OnBlendBegin.Invoke();

            float t = 0;
            while (true)
            {
                t += Time.fixedDeltaTime * speed;

                OnBlending(t);
                ShowMessage("Blender Value is = " + t + " of 1");

                if (interfaces != null)
                    foreach (var response in interfaces)
                        response.OnBlending(t);

                UpdateLoop(ref t);

                yield return null;
            }
        }
        #endregion Blending Mechanics
    }
}