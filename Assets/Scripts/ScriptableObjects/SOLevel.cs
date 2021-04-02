using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOLevel : ScriptableObject
{
    public SOEvent[] events;
    public int startingScore;
    public float spawningInterval;
}
