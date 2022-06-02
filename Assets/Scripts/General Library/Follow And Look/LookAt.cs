using UnityEngine;

namespace GeneralLibrary
{
    public class LookAt : Debuggable
    {
        [System.Flags]
        public enum Axis
        {
            X = 0001,
            Y = 0010,
            Z = 0100
        }

        [SerializeField] private Axis axis = Axis.X | Axis.Y | Axis.Z;
        [SerializeField] private float speed = 1.00f;
        [SerializeField] protected Transform lookTarget;
        /// <summary>
        /// return look direction 
        /// </summary>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        private Vector3 GetLookDirection(Vector3 targetPoint)
        {
            if ((axis & Axis.X) != Axis.X)
                targetPoint.x = transform.position.x;

            if ((axis & Axis.Y) != Axis.Y)
                targetPoint.y = transform.position.y;

            if ((axis & Axis.Z) != Axis.Z)
                targetPoint.z = transform.position.z;

            Vector3 lookDir = targetPoint - transform.position;
            return lookDir.normalized;

        }

        public void LookTo(Transform target)
        {
            Vector3 lookDirection = GetLookDirection(target.position);
            transform.forward = lookDirection;
        }
        public void Look()
        {
            Vector3 lookDirection = GetLookDirection(lookTarget.position);
            transform.forward = lookDirection;
            ShowMessage("Looking To  = " + lookTarget.name);
        }
        public void ChangeTarget(Transform newLookTarget)
        {
            if (lookTarget && newLookTarget)
                ShowMessage("LookTarget = '" + lookTarget.name + "' Switch to = '" + newLookTarget.name + "'");
            lookTarget = newLookTarget;
        }

        protected virtual void LateUpdate()
        {
            if (lookTarget)
            {
                Vector3 lookDirection = GetLookDirection(lookTarget.position);
                transform.forward = Vector3.Lerp(transform.forward, lookDirection, speed * Time.fixedDeltaTime);
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (lookTarget == null)
                ShowMessage("<color=cyan>Look Target field is null</color>");

            if (useDebug)
                LateUpdate();
        }
#endif

    }
}