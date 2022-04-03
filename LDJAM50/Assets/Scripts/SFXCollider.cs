using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCollider : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}