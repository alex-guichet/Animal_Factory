using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShadowManager : MonoBehaviour
{
    public GameObject shadow;
    private SpriteRenderer _shadowRenderer;
    private float _shadowAlphaInit;
    private bool _fade;
    private bool _activateFade;
    private float _fadeMin;


    private void OnEnable()
    {
        ShowShadow();
    }

    private void Awake()
    {
        if (transform.Find("Shadow") != null)
        {
            shadow = transform.Find("Shadow").gameObject;
            _shadowRenderer = shadow.GetComponent<SpriteRenderer>();
            _shadowAlphaInit = _shadowRenderer.color.a;
        }
    }

    private void FixedUpdate()
    {
        if (_activateFade)
        {
            Fade(_fade);
        }
    }

    public void HideShadow()
    {
        shadow.SetActive(false);
    }

    private void ShowShadow()
    {
        shadow.SetActive(true);
    }

    private void Fade(bool fade)
    {
        if (fade)
        {
            if (_shadowRenderer.color.a > _fadeMin)
            {
                Color color = _shadowRenderer.color;
                color.a -= Time.deltaTime;
                _shadowRenderer.color = color;
            }
            else
            {
                _activateFade = false;
            }
        }
        else
        {
            if (_shadowRenderer.color.a <= _shadowAlphaInit)
            {
                Color color = _shadowRenderer.color;
                color.a += Time.deltaTime;
                _shadowRenderer.color = color;
            }
            else
            {
                _activateFade = false;
            }
        }
    }

    public void FadeOut(float fadeMin)
    {
        _fade = true;
        _activateFade = true;
        _fadeMin = fadeMin;
    }

    public void FadeIn()
    {
        _fade = false;
        _activateFade = true;
    }

    public void SetShadow()
    {
        shadow = transform.Find("Shadow").gameObject;
    }
}
