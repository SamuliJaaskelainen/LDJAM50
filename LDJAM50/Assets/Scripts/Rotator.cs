using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 angleSpeeds;

    void Update()
    {
        transform.Rotate(angleSpeeds * Time.deltaTime, Space.Self);
    }
}
