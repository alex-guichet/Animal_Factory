using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{

}

[System.Serializable]
public class LevelPreset
{
    [FormerlySerializedAs("_levelNumber")] public int levelNumber;
    [FormerlySerializedAs("_livesNUmber")] public int livesNUmber;
    [FormerlySerializedAs("_levelSpeed")] public int levelSpeed;
}
