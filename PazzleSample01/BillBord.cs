using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBord : MonoBehaviour
{
    GameObject mainCamera;
    [SerializeField] bool reverse = false;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

   
    void Update()
    {
        if (!reverse)
        {
            transform.forward = mainCamera.transform.position;
        }
        else
        {
            transform.forward = mainCamera.GetComponent<Camera>().transform.forward;
        }
    }
}
