using UnityEngine;

namespace GeneralLibrary
{

    public class Follow : Debuggable
    {
        #region Macros
        [System.Flags]
        private enum Axis
        {
            X = 0x01,
            Y = 0x10,
            Z = 0100
        }

        [SerializeField]
        private enum FollowSpace
        {
            WorldToWorld,
            WorldToCanvus
        }
        #endregion Macros

        #region Propertys
        [Header("Follow Setup")]
        [SerializeField, Tooltip("Follow Axis")] private FollowSpace followSpace = FollowSpace.WorldToWorld;
        [SerializeField, Tooltip("Follow Axis")] private Axis axis = Axis.X | Axis.Y | Axis.Z;
        [SerializeField, Tooltip("Follower Object")] private Transform folower = null;
        [SerializeField, Tooltip("Follow Speed")] private float speed = 5;
        private Camera cam = null;

        [Header("Offset Setup")]
        [SerializeField] private bool useInitialOffset = false;
        [SerializeField] private Vector3 offset = new Vector3();

        #endregion Propertys

        #region Initialize
        private void Awake()
        {
            if (folower && useInitialOffset)
            {
                switch (followSpace)
                {
                    case FollowSpace.WorldToWorld:
                        offset = folower.position - transform.position;
                        break;
                    case FollowSpace.WorldToCanvus:
                        offset = folower.position - GetCamera.WorldToScreenPoint(transform.position);
                        break;
                }
            }
        }

        private Camera GetCamera
        {
            get
            {
                if (cam == null)
                {
                    cam = Camera.main;
                    if (cam == null)
                        ShowMessage("FollowSpace selected to 'World to Canvus' mode but Camera is null of " + name);
                }

                return cam;
            }
        }
        #endregion Initialize

        #region Editor Debug
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (useDebug)
                LateUpdate();
        }
#endif

        #endregion Editor Debug

        #region Follow Mechanics
        /// <summary>
        /// return a position where folower should move
        /// </summary>
        /// <returns></returns>
        private Vector3 GetPoint()
        {
            Vector3 point;

            switch (followSpace)
            {
                case FollowSpace.WorldToWorld:
                    point = transform.position;
                    break;
                case FollowSpace.WorldToCanvus:
                    point = GetCamera.WorldToScreenPoint(transform.position);
                    break;
                default:
                    point = transform.position;
                    break;
            }
            point += offset;//Add offset valus

            //Ignore axis to folow.
            if ((axis & Axis.X) != Axis.X)
                point.x = folower.position.x;
            if ((axis & Axis.Y) != Axis.Y)
                point.y = folower.position.y;
            if ((axis & Axis.Z) != Axis.Z)
                point.z = folower.position.z;

            return point;
        }
        private void LateUpdate()
        {
            if (folower == null)
                ShowMessage("Follower is null to folow this object");

            if (folower)
                folower.position = Vector3.MoveTowards(folower.position, GetPoint(), speed * Time.fixedDeltaTime);//update folower position
        }
        #endregion Follow Mechanics
    }
}
