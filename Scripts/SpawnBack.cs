using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnBack : MonoBehaviour
{
    [SerializeField] private Transform startPos;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.transform.position = startPos.position;
    }
}
