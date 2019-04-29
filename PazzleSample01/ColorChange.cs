using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] Material defMaterial;
    [SerializeField] Material changeMaterial;
    Renderer ren;

    GameObject gameMaster;

    [SerializeField] bool changeKey;


    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        ren = GetComponent<Renderer>();
    }
        
    void Update()
    {
        changeKey = gameMaster.GetComponent<GameMaster>().rainbowMode;

        if(!changeKey)
        {
            ren.material = defMaterial;
        }
        else
        {
            ren.material = changeMaterial;
        }
    }
}
