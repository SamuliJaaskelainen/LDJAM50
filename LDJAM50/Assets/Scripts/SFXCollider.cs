using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCollider : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;

    float timer;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (timer < Time.time)
        {
            timer = Time.time + 0.33f;
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
    }
}