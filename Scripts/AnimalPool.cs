using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Serialization;

public enum AnimalType
{
    Target,
    Misplaced,
    Errors
}

public class AnimalPool : MonoBehaviour
{
    public List<Animals> animalList;
    [SerializeField] private GameObject animalTemplate;

    private List<GameObject> _currentAnimalList;
    private GameObject _shadow;

    private CapsuleCollider _animalTCollider;
    private CapsuleCollider _animalTColliderMesh;
    private AnimalMovement _animalTMovement;
    private ConstantForce _animalTConstantForce;
    private Rigidbody _animalTRigidbody;
    private int _animalTLayer;
    private int _animalTLayerMesh;
    private string _animalTtag;

    void Awake()
    {
        _animalTRigidbody = animalTemplate.GetComponent<Rigidbody>();
        _animalTCollider = animalTemplate.GetComponent<CapsuleCollider>();
        _animalTColliderMesh = animalTemplate.transform.Find("Mesh").gameObject.GetComponent<CapsuleCollider>();
        _animalTMovement = animalTemplate.GetComponent<AnimalMovement>(); ;
        _animalTConstantForce = animalTemplate.GetComponent<ConstantForce>();
        _animalTLayer = animalTemplate.layer;
        _animalTLayerMesh = animalTemplate.transform.Find("Mesh").gameObject.layer;
        _animalTtag = animalTemplate.tag;

        _shadow = animalTemplate.transform.Find("Shadow").gameObject;

        _currentAnimalList = new List<GameObject>();

        foreach (Animals a in animalList)
        {
            for(int i = 0; i < a.numberTotal; i++)
            {
                GameObject animal = FormatAnimal(Instantiate(a.gameObject, transform));
                animal.SetActive(false);
                _currentAnimalList.Add(animal);
            }
        }
    }

    public GameObject GetAnimal(string animalName)
    {
        foreach(GameObject g in _currentAnimalList)
        {
            if (g.name.Contains(animalName) && !g.activeInHierarchy)
            {
                return g;
            }
        }

        GameObject animal = Instantiate(FindAnimalObject(animalName), transform);
        animal.SetActive(false);
        _currentAnimalList.Add(animal);
        return animal;
    }

    private GameObject FindAnimalObject(string animalName)
    {
        foreach(Animals a in animalList)
        {
            if (a.gameObject.name.Contains(animalName))
            {
                return a.gameObject;
            }
        }
        return null;
    }

    private GameObject FormatAnimal(GameObject animal)
    {
        GameObject shadow = Instantiate(_shadow, animal.transform.position, _shadow.transform.rotation, animal.transform);
        shadow.name = _shadow.name;

        Destroy(animal.GetComponent<Animator>());

        animal.AddComponent<Rigidbody>();
        animal.GetComponent<Rigidbody>().angularDrag = _animalTRigidbody.angularDrag;

        CapsuleCollider animalCollider = animal.GetComponent<CapsuleCollider>();
        animalCollider.enabled = false;
        animalCollider.isTrigger = _animalTCollider.isTrigger;
        animalCollider.center = _animalTCollider.center;
        animalCollider.radius = _animalTCollider.radius;
        animalCollider.height = _animalTCollider.height;

        animal.AddComponent<ConstantForce>();
        animal.GetComponent<ConstantForce>().force = _animalTConstantForce.force;

        animal.AddComponent<AnimalMovement>();
        AnimalMovement animalMovement = animal.GetComponent<AnimalMovement>();
        animalMovement.GlobalSpeed = _animalTMovement.GlobalSpeed;
        animalMovement.AnimalEjectInsideSpeed = _animalTMovement.AnimalEjectInsideSpeed;
        animalMovement.AnimalEjectOutsideSpeed = _animalTMovement.AnimalEjectOutsideSpeed;
        animalMovement.AnimalTorqueSpeed = _animalTMovement.AnimalTorqueSpeed;

        animal.AddComponent<ShadowManager>();

        animal.transform.Find("Mesh").gameObject.AddComponent<CapsuleCollider>();
        CapsuleCollider animalColliderMesh = animal.transform.Find("Mesh").gameObject.GetComponent<CapsuleCollider>();
        animalColliderMesh.isTrigger = _animalTColliderMesh.isTrigger;
        animalColliderMesh.center = _animalTColliderMesh.center;
        animalColliderMesh.radius = _animalTColliderMesh.radius;
        animalColliderMesh.height = _animalTColliderMesh.height;

        animal.GetComponent<ShadowManager>().SetShadow();

        animal.layer = _animalTLayer;
        animal.transform.Find("Mesh").gameObject.layer = _animalTLayerMesh;
        animal.tag = _animalTtag;

        return animal;
    }

}

[System.Serializable]
public class Animals
{
    [FormerlySerializedAs("_gameObject")] public GameObject gameObject;
    [FormerlySerializedAs("_type")] public AnimalType type;
    [FormerlySerializedAs("_numberTotal")] public int numberTotal;
    [FormerlySerializedAs("_numberCount")] public int numberCount;

    public Animals(GameObject gameObject, int numberTotal, int numberCount)
    {
        this.gameObject = gameObject;
        this.numberTotal = numberTotal;
        this.numberCount = numberCount;
    }
}
