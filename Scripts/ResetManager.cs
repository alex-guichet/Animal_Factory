using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResetManager : MonoBehaviour
{
    [SerializeField] private Transform animalPool;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ChangeColor changeColor;

    // Start is called before the first frame update
    public void Reset()
    {
        DeactivateActiveAnimals();
        spawnManager.Initialize();
        scoreManager.Initialize();
        changeColor.Initialize();
        uiManager.DestroyObjectCounts();
        soundManager.gameMusic.Play();
        spawnManager.StartSpawn();
    }

    private void DeactivateActiveAnimals()
    {
        for(int i = 0; i < animalPool.childCount; i++)
        {
            if (animalPool.GetChild(i).gameObject.activeInHierarchy)
            {
                animalPool.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
