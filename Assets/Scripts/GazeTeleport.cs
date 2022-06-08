using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GazeTeleport : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    [SerializeField] private float maxDistane = 500;
    [SerializeField] private LayerMask targetLayer = 0;
    [SerializeField] private Transform teleportable = null;

    private Transform destination = null;
    [SerializeField] private Image fillImage = null;

    private IEnumerator Load()
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        if (fillImage)
            fillImage.enabled = true;

        Debug.Log("Start");
        //updating fill rate of indicator image
        while (Time.time < endTime)
        {
            if(fillImage)
                fillImage.fillAmount = Mathf.InverseLerp(startTime, endTime, Time.time);
            yield return null;
        }

        Debug.Log("End");
        //teleport to the target destination
        if(teleportable)
            teleportable.position = new Vector3(destination.position.x, teleportable.position.y, destination.position.z);
       // this.destination = null;

        if (fillImage)
        {
            fillImage.fillAmount = 0;
            fillImage.enabled = false;
        }
    }


    private void Update()
    {
        var target = GetDestination();

        if (destination == null)
        {
            //find new  destination point
            StopAllCoroutines();
            destination = target;
            if (destination)
                StartCoroutine(Load());
        }
        else
        {
            if (destination != target)
            {
                //cancel teleport to the previous selected target
                StopAllCoroutines();
                destination = null;

                if (fillImage)
                {
                    fillImage.fillAmount = 0;
                    fillImage.enabled = false;
                }
            }
        }
    }


    /// <summary>
    /// retrn target tranform which direction player is facing
    /// </summary>
    /// <returns></returns>
    private Transform GetDestination()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistane, targetLayer))
            return hit.transform;

        return null;
    }
}
