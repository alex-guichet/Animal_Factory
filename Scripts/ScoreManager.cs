using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private int livesNumber;
    [SerializeField] private float levelSpeed = 9f;
    [SerializeField] private float speedLabel;
    [SerializeField] private int incrementScore = 5;
    [SerializeField] private int decrementScore = 10;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ChangeColor screenColor;
    [SerializeField] private ChangeColor lightColor;
    [SerializeField] private FloatVariable globalSpeed;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private UnityEvent onFinish;
    [SerializeField] private Color[] colorsSpeed;


    private int _currentScore = 0;
    private int _currentLives;
    private int _remainingAnimalsCount;
    private int _checkAnimalCount;
    private int _spawnCountInit;
    private bool _gameOver;

    public int LevelNumber
    {
        get { return levelNumber; }
    }

    public void Initialize()
    {
        _currentScore = 0;
        _checkAnimalCount = 0;
        _currentLives = livesNumber;
        uiManager.scoreLabel.text = "0";
        _spawnCountInit = spawnManager.spawnCount;
        _remainingAnimalsCount = _spawnCountInit;
        uiManager.countLabel.text = _currentLives.ToString() + "/" + livesNumber.ToString();
        uiManager.countRemainingLabel.text = _remainingAnimalsCount.ToString();
        globalSpeed.value = levelSpeed;
        Color color = uiManager.stars.transform.Find("Star1").transform.GetComponent<Image>().color;
        uiManager.stars.transform.Find("Star2").transform.GetComponent<Image>().color = color;
        uiManager.stars.transform.Find("Star3").transform.GetComponent<Image>().color = color;

    }

    private void Awake()
    {
        SpeedColor();
        uiManager.livesLabel.text = livesNumber.ToString();
        globalSpeed.value = levelSpeed;
        _checkAnimalCount = 0;
        _currentLives = livesNumber;
        uiManager.scoreLabel.text = "0";
    }
    
    private void Start()
    {
        _spawnCountInit = spawnManager.spawnCount;
        _remainingAnimalsCount = _spawnCountInit;
        uiManager.countLabel.text = _currentLives.ToString()+ "/"+ livesNumber.ToString();
        uiManager.countRemainingLabel.text = _remainingAnimalsCount.ToString();
        uiManager.textLevel.text = "Level " + levelNumber.ToString();
    }

    private void FixedUpdate()
    {
        if (_gameOver && !soundManager.gameOverBeep.isPlaying)
        {
            soundManager.gameOverMusic.Play();
            uiManager.gameOverUI.SetActive(true);
            _gameOver = false;
        }
    }

    public bool AnimalCheck(GameObject[] checkedAnimals, string animalName)
    {
        foreach (GameObject a in checkedAnimals)
        {
            if (animalName.Contains(a.name))
            {
                spawnManager.IncrementAnimalCount(a.name);
                IncreaseScore();
                return true;
            }
        }
        DecreaseScore();
        return false;
    }
    
    private void IncreaseScore()
    {
        _currentScore += incrementScore;
        uiManager.scoreLabel.text = _currentScore.ToString();
        screenColor.ColorGood();
        lightColor.ColorGood();
        soundManager.goodBeep.Play();
        IncrementCheckedAnimalCount();
    }

    private void DecreaseScore()
    {
        if (_currentLives > 0)
        {
            _currentLives--;

            uiManager.countLabel.text = _currentLives.ToString() + "/" + livesNumber.ToString();

            _currentScore -= decrementScore;

            if (_currentScore < 0)
            {
                _currentScore = 0;
            }
            screenColor.ColorBad();
            lightColor.ColorBad();
            uiManager.scoreLabel.text = _currentScore.ToString();
            soundManager.badBeep.Play();

            if (_currentLives == 0)
            {
                GameOver();
            }
        }
        IncrementCheckedAnimalCount();
    }

    public void DecrementRemainingCount()
    {
        _remainingAnimalsCount--;
        uiManager.countRemainingLabel.text = _remainingAnimalsCount.ToString();
    }

    public void IncrementCheckedAnimalCount()
    {
        _checkAnimalCount++;
        if (_checkAnimalCount == _spawnCountInit)
        {
            Success();
        }
    }

    private void GameOver()
    {
        onFinish.Invoke();
        globalSpeed.value = 0f;
        lightColor.GameOver();
        soundManager.gameOverBeep.Play();
        StartCoroutine(Flicker(uiManager.countLabel.gameObject));
        soundManager.gameMusic.Stop();
        uiManager.SetObjectsUI(uiManager.objectCount, uiManager.objectCountsGameOver, spawnManager.animals);
        uiManager.scoreValueGameOver.text = _currentScore.ToString();
        _gameOver = true;
    }

    private void Success()
    {
        onFinish.Invoke();
        uiManager.scoreValueSuccess.text = _currentScore.ToString();

        Color color = uiManager.stars.transform.Find("Star1").transform.GetComponent<Image>().color;
        color.a = 0.2f;

        float percentageLives = (float)_currentLives / livesNumber;
        int countStars;
        countStars = 3;

        if (0.6f <= percentageLives && percentageLives < 1)
        {
            uiManager.stars.transform.Find("Star3").transform.GetComponent<Image>().color = color;
            countStars = 2;
        }
        else if(percentageLives < 0.6f)
        {
            uiManager.stars.transform.Find("Star2").transform.GetComponent<Image>().color = color;
            uiManager.stars.transform.Find("Star3").transform.GetComponent<Image>().color = color;
            countStars = 1;
        }

        uiManager.SetObjectsUI(uiManager.objectCount, uiManager.objectCountsSuccess, spawnManager.animals);

        soundManager.gameMusic.Stop();
        soundManager.successMusic.Play();
        uiManager.successUI.SetActive(true);

        string keyName = "CountStarsLv" + levelNumber.ToString();
        if (PlayerPrefs.HasKey(keyName))
        {
            if(countStars > PlayerPrefs.GetInt(keyName))
            {
                PlayerPrefs.SetInt(keyName, countStars);
            }
        }
        else
        {
            PlayerPrefs.SetInt(keyName, countStars);
        }
    }

    IEnumerator Flicker(GameObject gameObject)
    {
        for(int i=0; i < 5; i++)
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SpeedColor()
    {
        switch (speedLabel)
        {
            case 1:
                uiManager.speedValue.color = colorsSpeed[0];
                break;
            case 2:
                uiManager.speedValue.color = colorsSpeed[1];
                break;
            case 3:
                uiManager.speedValue.color = colorsSpeed[2];
                break;
            case 4:
                uiManager.speedValue.color = colorsSpeed[3];
                break;
            case 5:
                uiManager.speedValue.color = colorsSpeed[4];
                break;
        }
        uiManager.speedValue.text = speedLabel.ToString();
    }

    public void Pause()
    {
        globalSpeed.value = 0f;
    }

    public void UnPause()
    {
        globalSpeed.value = levelSpeed;
    }
}
