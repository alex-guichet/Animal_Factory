using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    public float value;
}
