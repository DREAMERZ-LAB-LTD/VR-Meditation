using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInitilaizer : MonoBehaviour
{
    private static bool initialized = false;
    [SerializeReference] private List<GameObject> portals = new List<GameObject>();
    private void Awake()
    {
        if (initialized)
        {
            foreach (var portal in portals)
                if (portal)
                    portal.SetActive(true);
        }
        else
            initialized = true;
    }
}
