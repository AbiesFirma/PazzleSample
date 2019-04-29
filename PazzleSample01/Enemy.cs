using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject gameMaster;

    public int enemyNumber;
    public enum EnemyColor
    {
        RED, BLUE, GREEN, YELLOW, BLACK
    };
    public EnemyColor enemyColor;

    [SerializeField] Text numberText;

    [SerializeField] float moveSpeed = 1.0f;
    float stopTime;

    [SerializeField] GameObject enemyDestroyParticle;
    [SerializeField] int minNumber = 5;
    [SerializeField] int maxNumber = 20;

    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameMaster = GameObject.Find("GameMaster");
        enemyNumber = Random.Range(minNumber, maxNumber);
        moveSpeed *= Random.Range(0.5f, 1.5f);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        numberText.text = string.Format("{0}", enemyNumber);

        //DestoryEnemy
        if(enemyNumber <= 0)
        {
            var overKill = enemyNumber;
            enemyNumber = 0;
            gameMaster.GetComponent<GameMaster>().EnemyDestroy(-overKill);

            Instantiate(enemyDestroyParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        //move
        if (moveSpeed > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if(moveSpeed < 0) transform.rotation = Quaternion.Euler(0, 180, 0);


        if (rb.velocity.x < 0.5f && rb.velocity.x > -0.5f)
        {
            stopTime += Time.deltaTime;

            if (stopTime > 2.0f)
            {
                moveSpeed *= -1;
                stopTime = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveSpeed, 0, 0);
        rb.velocity = move;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall") moveSpeed *= -1;
    }
}
