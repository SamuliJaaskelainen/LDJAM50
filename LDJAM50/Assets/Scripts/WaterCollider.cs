using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}