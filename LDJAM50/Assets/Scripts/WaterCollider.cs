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

    public void Splash()
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
        VfxPlayer.Instance.PlayVfx(3, transform.position);
    }
}