using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Serialization;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private bool textMeshPro;
    [SerializeField] private Color colorGood;
    [SerializeField] private Color colorBad;
    [SerializeField] private float durationFlicker = 0.5f;

    private Color[] _colorInit;
    private bool _isRunning;
    private List<string> _runList;
    private bool _gameOver;

    public void Initialize()
    {
        _isRunning = false;
        _gameOver = false;
    }

    private void Awake()
    {
        _runList = new List<string>();
        _colorInit = new Color[objects.Length];
        int colorIndex = 0;

        if (textMeshPro)
        {
            foreach (GameObject o in objects)
            {
                _colorInit[colorIndex] = o.GetComponent<TextMeshPro>().color;
                colorIndex++;
            }
        }
        else
        {
            foreach (GameObject o in objects)
            {
                Material material = o.GetComponent<Renderer>().material;
                _colorInit[colorIndex] = material.color;
                colorIndex++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isRunning && _runList.Count > 0 && !_gameOver)
        {
            if(_runList.ElementAt(0) == "g")
            {
                StartCoroutine(FlickerColor(colorGood));
            }
            else
            {
                StartCoroutine(FlickerColor(colorBad));
            }
            _runList.RemoveAt(0);
        }
    }

    public void GameOver()
    {
        _gameOver = true;
        Color color = colorBad;

        foreach (GameObject o in objects)
        {
            Material material = o.GetComponent<Renderer>().material;
            color.a = material.color.a;
            material.color = color;
            o.GetComponent<Renderer>().material = material;
        }

    }

    public void ColorBad()
    {
        _runList.Add("b");
    }

    public void ColorGood()
    {
        _runList.Add("g");
    }

    IEnumerator FlickerColor(Color color)
    {
        _isRunning = true;

        if (textMeshPro)
        {
            foreach (GameObject o in objects)
            {
                Color colorLocal = o.GetComponent<TextMeshPro>().color;
                color.a = colorLocal.a;
                o.GetComponent<TextMeshPro>().color = color;
            }
        }
        else
        {
            foreach (GameObject o in objects)
            {
                Material material = o.GetComponent<Renderer>().material;
                color.a = material.color.a;
                material.color = color;
                o.GetComponent<Renderer>().material = material;
            }
        }

        yield return new WaitForSeconds(durationFlicker);

        int colorIndex = 0;

        if (textMeshPro)
        {
            foreach (GameObject o in objects)
            {
                o.GetComponent<TextMeshPro>().color = _colorInit[colorIndex];
                colorIndex++;
            }
        }
        else
        {
            foreach (GameObject o in objects)
            {
                Material material = o.GetComponent<Renderer>().material;
                material.color = _colorInit[colorIndex];
                colorIndex++;
                o.GetComponent<Renderer>().material = material;
            }
        }

        _isRunning = false;
    }

}
