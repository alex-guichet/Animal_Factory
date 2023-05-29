using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class BounceOff : MonoBehaviour
{
    [SerializeField] private float forceBounce;
    [SerializeField] private Vector3 bounceDirection;

    private Rigidbody _animalRb;

    void Awake()
    {
        _animalRb = GetComponent<Rigidbody>();
    }


    public void Bounce()
    {
        _animalRb.AddForce(bounceDirection * forceBounce, ForceMode.Impulse);
    }
}
