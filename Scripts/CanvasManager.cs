using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelector;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private SoundManager soundManager;
    
    private static bool _mainMenuVisited;

    private void Awake()
    {
        if (_mainMenuVisited)
        {
            levelSelector.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            _mainMenuVisited = true;
        }
    }
}
