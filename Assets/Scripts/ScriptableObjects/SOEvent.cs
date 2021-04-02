using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOEvent : ScriptableObject
{
    public new string name;
    public GameObject car;
    public float timeLimit;
    public SOTask[] tasks;
    public int onFail;
    public int onSuccess;
    public Sprite carIcon;
}
