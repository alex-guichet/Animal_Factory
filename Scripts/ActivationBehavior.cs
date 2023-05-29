using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActivationBehavior : MonoBehaviour
{
    //Shown Variables

    [SerializeField] private bool exited;
    [SerializeField] private bool piped;
    [SerializeField] private bool shadow;
    [SerializeField] private bool shadowTube;
    [SerializeField] private float shadowFadeMin = 0;
    [SerializeField] private ChangeColor pipeColor;
    [SerializeField] private bool grounded;
    [SerializeField] private GameObject[] checkedAnimals;
    [SerializeField] private Transform[] spawnPointsPets;

    //Hidden Variables
    private ScoreManager _scoreManager;
    private Transform _petTransform;
    private bool _hasEntered;

    private void Awake()
    {
        if(checkedAnimals == null)
        {
            checkedAnimals = new GameObject[0];
        }

        if (piped)
        {
            _petTransform = transform.Find("Pet");
            GameObject animal = Instantiate(checkedAnimals[0], _petTransform.transform.position, _petTransform.transform.rotation, _petTransform);
            animal.GetComponent<Animator>().enabled = false;
            animal.GetComponent<Collider>().enabled = false;
        }

        if (exited)
        {
            for(int i = 0; i < checkedAnimals.Length; i++)
            {
                GameObject animal = Instantiate(checkedAnimals[i], spawnPointsPets[i].position, spawnPointsPets[i].rotation, spawnPointsPets[i]);
                animal.GetComponent<Animator>().enabled = false;
                animal.GetComponent<Collider>().enabled = false;
                spawnPointsPets[i].Find("Shadow").gameObject.SetActive(true);
            }


        }
    }

    private void Start()
    {
        _scoreManager = GameObject.Find("LevelManager").GetComponent<ScoreManager>();


    }


    private void OnTriggerExit(Collider other)
    {
        if (_hasEntered)
        {
            other.transform.parent.GetComponent<Collider>().enabled = true;
            _scoreManager.DecrementRemainingCount();
        }

        if (exited)
        {
            _scoreManager.AnimalCheck(checkedAnimals, other.transform.parent.name);
            other.transform.parent.gameObject.SetActive(false);
        }

        if (shadow)
        {
            other.transform.parent.GetComponent<ShadowManager>().FadeIn();
        }

        if (shadowTube)
        {
            if (other.CompareTag("Animal"))
            {
                other.GetComponent<ShadowManager>().FadeIn();
            }
        }

        if (grounded)
        {
            other.transform.parent.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (exited)
        {
            other.transform.parent.GetComponent<Collider>().enabled = false;
            other.transform.parent.GetComponent<ShadowManager>().FadeOut(shadowFadeMin);
        }

        if (_hasEntered)
        {
            other.transform.parent.GetComponent<Collider>().enabled = false;
            other.transform.parent.GetComponent<ShadowManager>().FadeOut(shadowFadeMin);
        }

        if (piped)
        {
            if (_scoreManager.AnimalCheck(checkedAnimals, other.transform.parent.name))
            {
                pipeColor.ColorGood();
            }
            else
            {
                pipeColor.ColorBad();
            }
            other.transform.parent.gameObject.SetActive(false);
        }

        if (grounded)
        {
            _scoreManager.AnimalCheck(checkedAnimals, other.transform.parent.name);
        }

        if (shadowTube)
        {
            if (other.CompareTag("Animal"))
            {
                other.GetComponent<ShadowManager>().FadeOut(shadowFadeMin);
            }
        }
    }
}
