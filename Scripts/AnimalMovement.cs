using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class AnimalMovement : MonoBehaviour
{

    [SerializeField] public FloatVariable globalSpeed;
    [SerializeField] private float animalEjectInsideSpeed = 40f;
    [SerializeField] private float animalEjectOutsideSpeed = 40f;
    [SerializeField] private float animalTorqueSpeed = 10f;


    private ShadowManager _shadowManager;
    private SoundManager _soundManager;

    private int _indexTube = 0;

    private bool _forward;
    private bool _ejectedInside;

    private Transform _pipeTransform;
    private Vector3 _randomTorque;
    private Rigidbody _animalRb;


    public FloatVariable GlobalSpeed
    {
        get { return globalSpeed; }
        set { globalSpeed = value; }
    }

    public float AnimalEjectInsideSpeed
    {
        get { return animalEjectInsideSpeed; }
        set { animalEjectInsideSpeed = value; }
    }

    public float AnimalEjectOutsideSpeed
    {
        get { return animalEjectOutsideSpeed; }
        set { animalEjectOutsideSpeed = value; }
    }

    public float AnimalTorqueSpeed
    {
        get { return animalTorqueSpeed; }
        set { animalTorqueSpeed = value; }
    }

    void OnDisable()
    {
        GetComponent<ConstantForce>().enabled = false;
        _animalRb.velocity = Vector3.zero;
        _forward = true;
        _ejectedInside = false;
        _randomTorque = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        _indexTube = 0;
        _animalRb.constraints = RigidbodyConstraints.FreezeRotation;
        _animalRb.velocity = Vector3.zero;
        _animalRb.useGravity = false;
    }

    void Awake()
    {
        _animalRb = GetComponent<Rigidbody>();
        _forward = true;
        _randomTorque = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
    }

    private void Start()
    {
        _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        _shadowManager = GetComponent<ShadowManager>();
    }

    void FixedUpdate()
    {
        if (_forward)
        {
            Forward();
        }
        else if (_ejectedInside)
        {
            EjectInTube();
        }
    }

    private void Forward()
    {
        _animalRb.velocity = transform.forward * globalSpeed.value;
    }

    public void ActivateEjectionInside(Transform pipeTransform)
    {
        _animalRb.constraints = RigidbodyConstraints.None;
        _animalRb.velocity = Vector3.zero;
        _animalRb.AddTorque(_randomTorque * animalTorqueSpeed, ForceMode.Acceleration);
        GetComponent<Collider>().enabled = false;
        _pipeTransform = pipeTransform;
        _forward = false;
        _ejectedInside = true;
    }

    private void EjectInTube()
    {
        _shadowManager.HideShadow();

        if (Vector3.Distance(transform.position, _pipeTransform.GetChild(_indexTube).position) > 0.3f)
        {
            _animalRb.MovePosition(Vector3.MoveTowards(transform.position, _pipeTransform.GetChild(_indexTube).position, animalEjectInsideSpeed * Time.deltaTime));
        }
        else if (_indexTube < _pipeTransform.childCount - 1)
        {
            _indexTube++;
            if(_indexTube == 1)
            {
                _soundManager._playWarp();
            }
        }
    }

    public void EjectOutside()
    {
        _shadowManager.HideShadow();
        GetComponent<Collider>().enabled = false;
        _forward = false;
        _animalRb.velocity = Vector3.zero;
        _animalRb.constraints = RigidbodyConstraints.None;
        _animalRb.useGravity = true;
        GetComponent<ConstantForce>().enabled = true;
        _animalRb.velocity = new Vector3(-1f, 1f, 0f) * animalEjectOutsideSpeed;
        _animalRb.AddTorque(_randomTorque * animalTorqueSpeed, ForceMode.Impulse);
    }
}
