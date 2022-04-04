using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VfxBloom : MonoBehaviour
{
    private Volume v;
    private Bloom b;
    
    [SerializeField] Player player;
    void Start()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out b);
    }
    void Update()
    {
        b.scatter.value = 1 - player.zoomLevel;
        //Debug.Log("Zoom Level:" + player.zoomLevel);
    }
}
