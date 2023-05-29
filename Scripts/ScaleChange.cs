using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(RectTransform))]
public class ScaleChange : MonoBehaviour
{
    [SerializeField] private float increaseSize;
    [SerializeField] private float increaseSpeed;

    private RectTransform _rectTransform;
    private Vector3 _targetScale;
    private Vector3 _initScale;
    [FormerlySerializedAs("_timer")] public float timer;
    private bool _goingBack;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initScale = _rectTransform.localScale;
        _targetScale = _initScale + (_initScale * increaseSize);
    }


    void FixedUpdate()
    {

        if(timer < 1 && !_goingBack)
        {
            timer += Time.deltaTime * increaseSpeed;
        }
        else if(timer > 0)
        {
            _goingBack = true;
            timer -= Time.deltaTime * increaseSpeed;
        }
        else
        {
            timer = 0;
            _goingBack = false;
        }

        _rectTransform.localScale = Vector3.Lerp(_initScale, _targetScale, Mathf.Clamp01(timer));
    }
}
