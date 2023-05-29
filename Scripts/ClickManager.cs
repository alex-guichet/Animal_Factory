using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ClickManager : MonoBehaviour
{
    //Shown Variables
    [SerializeField] private LayerMask animalPipeLayer;
    [SerializeField] private FloatVariable radiusSphereCast;
    [SerializeField] private SoundManager soundManager;

    //Hidden Variables
    private List<Transform> _pipeTransformList;
    private List<Transform> _animalTransformList;
    private Transform _currentAnimal;
    private Transform _currentPipe;
    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = EventSystem.current;
        _animalTransformList = new List<Transform>();
        _pipeTransformList = new List<Transform>();
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            foreach (Touch touch in Input.touches)
            {
                GameObject currentEvent = _eventSystem.currentSelectedGameObject;
                if(currentEvent == null)
                {
                    EjectAnimal(touch.position);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            GameObject currentEvent = _eventSystem.currentSelectedGameObject;
            if (currentEvent == null)
            {
                EjectAnimal(Input.mousePosition);
            }
        }
    }


    private void EjectAnimal(Vector3 touchPos)
    {
        _pipeTransformList.Clear();
        _animalTransformList.Clear();
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(ray, radiusSphereCast.value, Mathf.Infinity, animalPipeLayer);

        if (hits.Length > 1)
        {
            foreach (RaycastHit h in hits)
            {
                if (h.collider.CompareTag("Pipe") || h.collider.CompareTag("Outside"))
                {
                    _pipeTransformList.Add(h.collider.transform);
                }

                if (h.collider.CompareTag("Animal"))
                {
                    _animalTransformList.Add(h.collider.transform);
                }
            }

            if (_animalTransformList.Count > 0)
            {
                _currentAnimal = _animalTransformList.First();

                if (_pipeTransformList.Count > 0)
                {
                    float distance;
                    distance = Mathf.Abs(_pipeTransformList.First().position.z - _currentAnimal.position.z);

                    foreach (Transform p in _pipeTransformList)
                    {
                        float distanceLocal = Mathf.Abs((p.position.z - _currentAnimal.position.z));
                        if (distanceLocal <= distance)
                        {
                            _currentPipe = p;
                            distance = distanceLocal;
                        }
                    }
                    soundManager.bounceEffects[Random.Range(0, soundManager.bounceEffects.Length)].Play();

                    if (_currentPipe.CompareTag("Outside"))
                    {
                        _currentAnimal.GetComponent<AnimalMovement>().EjectOutside();
                        return;
                    }

                    _currentAnimal.GetComponent<AnimalMovement>().ActivateEjectionInside(_currentPipe);
                    
                }
                else
                {
                    _currentAnimal.GetComponent<AnimalMovement>().EjectOutside();
                    soundManager.bounceEffects[Random.Range(0, soundManager.bounceEffects.Length)].Play();
                }
            }
        }
        else if(hits.Length == 1)
        {
            foreach (RaycastHit h in hits)
            {
                if (h.collider.CompareTag("Animal"))
                {
                    h.collider.GetComponent<AnimalMovement>().EjectOutside();
                    soundManager.bounceEffects[Random.Range(0, soundManager.bounceEffects.Length)].Play();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {

    }

}
