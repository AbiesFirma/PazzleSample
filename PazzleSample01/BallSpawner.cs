using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    GameObject[] redBalls;
    GameObject[] blueBalls;
    GameObject[] greenBalls;
    GameObject[] yellowBalls;
    GameObject[] blackBalls;

    [SerializeField] GameObject redBallPrefab;
    [SerializeField] GameObject blueBallPrefab;
    [SerializeField] GameObject greenBallPrefab;
    [SerializeField] GameObject yellowBallPrefab;
    [SerializeField] GameObject blackBallPrefab;

    [SerializeField] GameObject[] spawnPoint;
    [SerializeField] int maxBalls = 15;

    float time = 0.0f;
    [SerializeField] float spawnTime = 1.0f;
    [SerializeField] float power = 2.0f;

    Rigidbody rb;
    AudioSource audioSource;
    GameObject gameMaster;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameMaster = GameObject.Find("GameMaster");
    }

    void Update()
    {
        var gmComp = gameMaster.GetComponent<GameMaster>();

        if (gmComp.gameActive)
        {

            time += Time.deltaTime;

            redBalls = GameObject.FindGameObjectsWithTag("Red");
            blueBalls = GameObject.FindGameObjectsWithTag("Blue");
            greenBalls = GameObject.FindGameObjectsWithTag("Green");
            yellowBalls = GameObject.FindGameObjectsWithTag("Yellow");
            blackBalls = GameObject.FindGameObjectsWithTag("Black");

            var redCount = redBalls.Length;
            var blueCount = blueBalls.Length;
            var greenCount = greenBalls.Length;
            var yellowCount = yellowBalls.Length;
            var blackCount = blackBalls.Length;

            var totalCount = redCount + blueCount + greenCount + yellowCount + blackCount;

            if (totalCount < maxBalls && time > spawnTime)
            {
                RandomSpawn();
                time = 0.0f;
            }
        }
    }


    void RandomSpawn()
    {
        GameObject[] ballPrefab = { redBallPrefab, blueBallPrefab, greenBallPrefab, yellowBallPrefab, blackBallPrefab };
        int ballNum = Random.Range(0, ballPrefab.Length);

        int pointNum = Random.Range(0, spawnPoint.Length);

        int dir = Random.Range(-10, 10);

        var ball = Instantiate(ballPrefab[ballNum], spawnPoint[pointNum].transform.position, Quaternion.identity);
        audioSource.Play();

        rb = ball.GetComponent<Rigidbody>();

        rb.AddForce(new Vector3(power * dir, 0, 0));

    }
}


