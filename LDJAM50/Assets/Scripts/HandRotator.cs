using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotator : MonoBehaviour
{
    Vector3 rot = new Vector3(3.0f, 7.2f, 5.75f);

    void Start()
    {
        transform.rotation = Random.rotation;
    }

    void Update()
    {
        transform.Rotate(rot * Time.deltaTime);
    }
}
