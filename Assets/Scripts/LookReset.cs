using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LookReset : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    Transform _OVRCameraRig;
    Transform _centreEyeAnchor;
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void OnEnable()=> SceneManager.sceneLoaded += ResetCameraOnSceneLoad;
    private void OnDisable()=>SceneManager.sceneLoaded -= ResetCameraOnSceneLoad;
    
    private void ResetCameraOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        OVRCameraRig ovr = FindObjectOfType<OVRCameraRig>();
        if (ovr)
        {
            _OVRCameraRig = ovr.transform;
            _centreEyeAnchor = ovr.centerEyeAnchor;
        }
        else
        {
            Debug.Log("No OVRCameraRig object found");
            return;
        }
   
        StartCoroutine(ResetCamera(initialPosition, initialRotation.eulerAngles.y));
        IEnumerator ResetCamera(Vector3 targetPosition, float targetYRotation)
        {

            yield return new WaitForEndOfFrame();

            float currentRotY = _centreEyeAnchor.eulerAngles.y;
            float difference = targetYRotation - currentRotY;
            _OVRCameraRig.Rotate(0, difference, 0);

            Vector3 newPos = new Vector3(targetPosition.x - _centreEyeAnchor.position.x, 0, targetPosition.z - _centreEyeAnchor.position.z);
            _OVRCameraRig.transform.position += newPos;
        }
    }
}
