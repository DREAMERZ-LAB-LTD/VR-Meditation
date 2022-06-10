using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    [SerializeField] private int fps = 70;
    private void Awake()
    {
        //Application.targetFrameRate = fps;
        //Time.fixedDeltaTime = 1 / (float)fps;
        //Time.maximumDeltaTime = 1 / (float)fps;
    }
}
