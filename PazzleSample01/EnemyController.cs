using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject[] enemys;
    GameObject mainCamera;

    GameObject gameMaster;

    [SerializeField] GameObject[] enemyPrefabs = new GameObject[5];
    [SerializeField] GameObject[] enemySpawnPoint;

    float spawnTime;
    AudioSource audioSource;


    void Start()
    {
        gameMaster = GameObject.Find("GameMaster");
        audioSource = GetComponent<AudioSource>();
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (enemys.Length != 0)
        {
            mainCamera.GetComponent<TouchRayTest>().targetEnemy = enemys[0];
        }
    }
        
    void Update()
    {
        var gmComp = gameMaster.GetComponent<GameMaster>();

        if (gmComp.gameActive)
        {

            enemys = GameObject.FindGameObjectsWithTag("Enemy");
            //Debug.Log(enemys.Length);

            if (mainCamera.GetComponent<TouchRayTest>().targetEnemy == null && enemys.Length != 0)
            {
                mainCamera.GetComponent<TouchRayTest>().targetEnemy = enemys[0];
            }


            //spawn
            if (enemys.Length < 3)
            {
                spawnTime += Time.deltaTime;

                if (spawnTime > 3.0f)
                {
                    var enemyNum = Random.Range(0, enemyPrefabs.Length);
                    var spawn = Random.Range(0, enemySpawnPoint.Length);
                    Instantiate(enemyPrefabs[enemyNum], enemySpawnPoint[spawn].transform.position, Quaternion.identity);
                    audioSource.Play();
                    spawnTime = 0.0f;
                }
            }
        }
    }

    
}
