using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public TextMeshPro scoreLabel;
    public TextMeshPro countLabel;
    public TextMeshPro countRemainingLabel;
    public TextMeshProUGUI scoreValueSuccess;
    public TextMeshProUGUI scoreValueGameOver;
    public GameObject stars;
    public GameObject objectCount;
    public GameObject objectStart;
    public Transform objectCountsSuccess;
    public Transform objectCountsGameOver;
    public Transform objectsStartTarget;
    public Transform objectsStartMisplaced;
    public GameObject gameOverUI;
    public GameObject successUI;
    public TextMeshPro speedValue;
    public TextMeshProUGUI speedUIValue;
    public TextMeshProUGUI numberAnimal;
    public SpawnManager spawnManager;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI livesLabel;

    private void Start()
    {
        numberAnimal.text = "Number : "+spawnManager.AnimalCount;
        speedUIValue.text = "Speed "+speedValue.text;
        SetObjectStart();
    }

    public void DestroyObjectCounts()
    {
        if(objectCountsSuccess.childCount > 0)
        {
            for (int i = 0; i < objectCountsSuccess.childCount; i++)
            {
                Destroy(objectCountsSuccess.GetChild(i).gameObject);
            }
        }

        if (objectCountsGameOver.childCount > 0)
        {
            for (int i = 0; i < objectCountsGameOver.childCount; i++)
            {
                Destroy(objectCountsGameOver.GetChild(i).gameObject);
            }
        }
    }

    public void SetObjectsUI(GameObject objectPrefab, Transform objectCounts, Animals[] animals)
    {
        foreach (Animals a in animals)
        {
            GameObject objectInst = Instantiate(objectPrefab, objectCounts);

            if(objectInst.GetComponent<TextMeshProUGUI>() != null)
            {
                objectInst.GetComponent<TextMeshProUGUI>().text = a.numberCount.ToString() + "/" + a.numberTotal.ToString();
            }

            GameObject animal = Instantiate(a.gameObject, objectInst.transform.Find("Object").transform.position, a.gameObject.transform.rotation, objectInst.transform.Find("Object").transform);
            animal.transform.Find("Mesh").gameObject.layer = LayerMask.NameToLayer("UI");
            animal.GetComponent<Animator>().enabled = false;
            animal.GetComponent<Collider>().enabled = false;
        }
    }

    private void SetObjectStart()
    {
        foreach(Animals a in spawnManager.animals)
        {
            Animals[] animals = new Animals[1];

            if (a.type == AnimalType.Target)
            {
                animals[0] = a;
                SetObjectsUI(objectStart, objectsStartTarget, animals);
            }

            if (a.type == AnimalType.Misplaced)
            {
                animals[0] = a;
                SetObjectsUI(objectStart, objectsStartMisplaced, animals);
            }
        }
    }
}
