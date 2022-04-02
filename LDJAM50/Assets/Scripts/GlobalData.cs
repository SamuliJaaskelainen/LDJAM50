using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static float seaLevel = 0.0f;

    void Awake()
    {
        seaLevel = 0.0f;
    }
}
