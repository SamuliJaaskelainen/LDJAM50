using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    [SerializeField] float riseAnimSpeed = 1.0f;

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0.0f, GlobalData.seaLevel, 0.0f), Time.deltaTime * riseAnimSpeed);
    }
}
