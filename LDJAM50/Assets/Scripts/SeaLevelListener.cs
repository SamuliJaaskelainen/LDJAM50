using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeaLevelListener : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioLowPassFilter>().cutoffFrequency = (22000 - (GlobalData.seaLevel)*2200);
    }
}
