using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedEnum : ScriptableObject
{
    public enum EnemyTypes
    {
        Simple,
        Strong,
        Chasing,
        Shooting,
    }
}
