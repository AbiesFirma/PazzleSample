using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotAnime : MonoBehaviour
{
    [SerializeField] float rotPower = 10.0f;
    [SerializeField] bool x = false;
    [SerializeField] bool y = false;
    [SerializeField] bool z = true;

    [SerializeField] bool scaleChange = false;
    [SerializeField] float scaleChangeSpeed = 1.0f;
    Vector3 defScale;
    float timer;

    void Start()
    {
        defScale = transform.localScale;
        timer = 0.0f;
    }
        
    void Update()
    {
        if (x)
        {
            transform.Rotate(rotPower, 0, 0);
        }
        if (y)
        {
            transform.Rotate(0, rotPower, 0);
        }
        if (z)
        {
            transform.Rotate(0, 0, rotPower);
        }


        if (scaleChange)
        {
            timer += Time.deltaTime;
            if (timer < 0.5f)
            {
                transform.localScale += new Vector3(0.1f * scaleChangeSpeed, 0.1f * scaleChangeSpeed, 0.1f * scaleChangeSpeed);
            }
            else if (timer > 0.5f && timer <= 1.0f)
            {
                transform.localScale -= new Vector3(0.1f * scaleChangeSpeed, 0.1f * scaleChangeSpeed, 0.1f * scaleChangeSpeed);
            }
            else if (timer > 1.0f || timer < 0.0f)
            {
                timer = 0.0f;
                transform.localScale = defScale;
            }
        }
    }
}
