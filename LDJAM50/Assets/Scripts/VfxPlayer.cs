using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxPlayer : MonoBehaviour
{
    public static VfxPlayer Instance;

    [SerializeField] List<GameObject> vfxs = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void PlayVfx(int index, Vector3 pos, int count)
    {
        GameObject vfx = Instantiate(vfxs[index], pos, vfxs[index].transform.rotation) as GameObject;
        ParticleSystem particleSystem = vfx.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.SetBurst(0, new ParticleSystem.Burst(0.0f, count));
        particleSystem.Play();
        Destroy(vfx, particleSystem.main.duration);
    }
}
