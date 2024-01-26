using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int TimeSecondsCount;

    public global::System.Int32 GetTime { get => TimeSecondsCount; }

    void Start()
    {
        InvokeRepeating("AddSecond", 0, 1);
    }
    internal void AddSecond()
    {
        TimeSecondsCount++;
    }
}
