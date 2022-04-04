using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static float seaLevel = 0.0f;
    public static int population = 0;
    public static int money = 0;
    public static int faith = 0;
    public static int rounds = 0;

    void Awake()
    {
        seaLevel = 0.0f;
        population = 0;
        money = 100;
        faith = 0;
        rounds = 0;
    }
}
