using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rotation : MonoBehaviour
{
    [SerializeField] private GameObject[] pulleys;
    [SerializeField] private FloatVariable scriptableSpeed;
    [SerializeField] private float speed = 6f;
    [SerializeField] private bool updateSpeed;
    [SerializeField] private bool backward;

    void FixedUpdate()
    {

        foreach(GameObject p in pulleys)
        {
            if (updateSpeed)
            {
                if (!backward)
                {
                    p.transform.Rotate(0f, scriptableSpeed.value, 0f, Space.Self);
                }
                else
                {
                    p.transform.Rotate(0f, -scriptableSpeed.value, 0f, Space.Self);
                }
            }
            else
            {
                p.transform.Rotate(0f, speed, 0f, Space.Self);
            }
        }
    }
}
