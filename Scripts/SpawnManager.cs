using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public enum SpawnStyle
{
    Ranks,
    RandomPos,
    DiagonalRight,
    DiagonalLeft,
    Mountains
}

public class SpawnManager : MonoBehaviour
{
    //Shown Variables
    public Animals[] animals;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private AnimalPool animalPool;
    [SerializeField] private SpawnStyleSelection[] spawnStyles;

    //Hidden Variables
    public int spawnCount { get; private set; }
    private float _timerSpawn;
    private float _timeSpawn;
    private float _timerSpawnStyle;
    private float _timeSpawnStyle;
    private float _timeSpawnInterval;
    private float _timeSpawnStyleInterval;
    private bool _start;
    private GameObject _currentAnimal;
    private List<string> _animalList;

    private int _lastindexSpawnStyle;
    private int _indexSpawn;
    private int _lastIndexSpawnPos;
    private bool _goingBackArrow;
    private SpawnStyleSelection _currentSpawnStyle;
    private GameObject _adsManager;

    public int AnimalCount
    {
        get { return _animalList.Count; }
    }

    public void Initialize()
    {
        _adsManager = GameObject.Find("AdsManager");
        if (_adsManager)
        {
            _adsManager.GetComponent<AdsManager>().UpdateAdCount();
        }
        _animalList.Clear();
        AnimalListInit();
        ResetNumberCount();
        spawnCount = _animalList.Count;
        _currentAnimal = null;
        _currentSpawnStyle = spawnStyles[Random.Range(0, spawnStyles.Length)];
        _timerSpawn = Random.Range(_currentSpawnStyle.timeSpawnInterval.x, _currentSpawnStyle.timeSpawnInterval.y);
        _timeSpawn = Time.timeSinceLevelLoad;
        _timerSpawnStyle = Random.Range(_currentSpawnStyle.durationInterval.x, _currentSpawnStyle.durationInterval.y);
        _timeSpawnStyle = Time.timeSinceLevelLoad + _timerSpawnStyle;
        _indexSpawn = 0;
        _lastIndexSpawnPos = 0;
        _lastindexSpawnStyle = 0;
        _start = false;
    }

    void Awake()
    {
        _animalList = new List<string>();
        Initialize();
    }

    void FixedUpdate()
    {
        if (_timeSpawn < Time.timeSinceLevelLoad && _start)
        {
            _timerSpawn = Random.Range(_currentSpawnStyle.timeSpawnInterval.x, _currentSpawnStyle.timeSpawnInterval.y);
            _timeSpawn = Time.timeSinceLevelLoad + _timerSpawn;
            SpawnPosition(_currentSpawnStyle.spawnStyle);
        }

        if (_timeSpawnStyle < Time.timeSinceLevelLoad && _start)
        {
            int indexSpawnStyle = Random.Range(0, spawnStyles.Length);

            while(indexSpawnStyle == _lastindexSpawnStyle && spawnStyles.Length > 1)
            {
                indexSpawnStyle = Random.Range(0, spawnStyles.Length);
            }
            _lastindexSpawnStyle = indexSpawnStyle;

            _currentSpawnStyle = spawnStyles[indexSpawnStyle];
            _timeSpawn = Time.timeSinceLevelLoad + _currentSpawnStyle.timeSpawnInterval.y;

            _timerSpawnStyle = Random.Range(_currentSpawnStyle.durationInterval.x, _currentSpawnStyle.durationInterval.y);
            _timeSpawnStyle = Time.timeSinceLevelLoad + _timerSpawnStyle;
        }
    }

    private void SpawnPosition(SpawnStyle spawnStyle)
    {
        switch (spawnStyle)
        {
            case SpawnStyle.Ranks:

                foreach(Transform s in spawnPoints)
                {
                    SpawnAnimals(s);
                }
                break;

            case SpawnStyle.RandomPos:

                int indexSpawnPos = Random.Range(0, spawnPoints.Length);
                while (indexSpawnPos == _lastIndexSpawnPos)
                {
                    indexSpawnPos = Random.Range(0, spawnPoints.Length);
                }
                _lastIndexSpawnPos = indexSpawnPos;

                Transform spawnPoint = spawnPoints[indexSpawnPos];
                SpawnAnimals(spawnPoint);
                break;

            case SpawnStyle.DiagonalRight:

                SpawnAnimals(spawnPoints[_lastIndexSpawnPos]);

                _lastIndexSpawnPos++;
                if(_lastIndexSpawnPos >= spawnPoints.Length)
                {
                    _lastIndexSpawnPos = 0;
                }
                break;

            case SpawnStyle.DiagonalLeft:

                _lastIndexSpawnPos--;
                if (_lastIndexSpawnPos < 0)
                {
                    _lastIndexSpawnPos = spawnPoints.Length-1;
                }
                SpawnAnimals(spawnPoints[_lastIndexSpawnPos]);
                break;

            case SpawnStyle.Mountains:

                if (_lastIndexSpawnPos < spawnPoints.Length-1 && !_goingBackArrow)
                {
                    _lastIndexSpawnPos++;
                }
                else if(_lastIndexSpawnPos > 0)
                {
                    _goingBackArrow = true;
                    _lastIndexSpawnPos--;
                }
                else
                {
                    _lastIndexSpawnPos++;
                    _goingBackArrow = false;
                }
                SpawnAnimals(spawnPoints[_lastIndexSpawnPos]);

                break;
        }

    }

    private void SpawnAnimals(Transform spawnPoint)
    {
        if (_indexSpawn < _animalList.Count)
        {
            _currentAnimal = animalPool.GetAnimal(_animalList.ElementAt(_indexSpawn));
            _currentAnimal.transform.position = spawnPoint.position;
            _currentAnimal.transform.rotation = spawnPoint.rotation;
            _currentAnimal.SetActive(true);
            _indexSpawn++;
        }
        else
        {
            _start = false;
        }
    }

    public void StartSpawn()
    {
        _timerSpawn = Random.Range(_currentSpawnStyle.timeSpawnInterval.x, _currentSpawnStyle.timeSpawnInterval.y);
        _timeSpawn = Time.timeSinceLevelLoad;
        _timerSpawnStyle = Random.Range(_currentSpawnStyle.durationInterval.x, _currentSpawnStyle.durationInterval.y);
        _timeSpawnStyle = Time.timeSinceLevelLoad + _timerSpawnStyle;
        _start = true;
    }

    public void StopSpawn()
    {
        _start = false;
        _timeSpawnInterval = _timeSpawn - Time.timeSinceLevelLoad;
        _timeSpawnStyleInterval = _timeSpawnStyle - Time.timeSinceLevelLoad;
    }

    public void ResumeSpawn()
    {
        _timeSpawn = Time.timeSinceLevelLoad + _timeSpawnInterval;
        _timeSpawnStyle = Time.timeSinceLevelLoad + _timeSpawnStyleInterval;
        _start = true;
    }

    private void AnimalListInit()
    {
        foreach (Animals a in animals)
        {
            for (int i = 0; i < a.numberTotal; i++)
            {
                _animalList.Add(a.gameObject.name);
            }
        }
        ShuffleList(_animalList);
    }

    private void ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void IncrementAnimalCount(string animalName)
    {
        foreach(Animals a in animals)
        {
            if(a.gameObject.name == animalName)
            {
                a.numberCount++;
            }
        }
    }

    public void ResetNumberCount()
    {
        foreach (Animals a in animals)
        {
             a.numberCount = 0;
        }
    }
}

[System.Serializable]
public class SpawnStyleSelection
{
    [FormerlySerializedAs("_spawnStyle")] public SpawnStyle spawnStyle;
    [FormerlySerializedAs("_timeSpawnInterval")] public Vector2 timeSpawnInterval;
    [FormerlySerializedAs("_durationInterval")] public Vector2 durationInterval;
}