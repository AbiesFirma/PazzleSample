using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public bool gameActive = false;
    float time = 0;

    public int score { get; private set; }
    public int killCount { get; private set; }

    GameObject targetEnemy;
    public string enemyColor { get; private set; }
    public int enemyNumber { get; private set; }
    [SerializeField]GameObject mainCamera;
    float chargePoint;
    [SerializeField] float chargeMax = 300.0f;
    [SerializeField] Image chargeGage;

    public bool rainbowMode = false;
    float rainbowTime;
    [SerializeField] float rainbowTimeMax = 30.0f;
    [SerializeField] GameObject rainbow;

    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip; //0.1.2;ballDaestroy, 3:enemyDestroy
    [SerializeField] GameObject rainbowClockAudio;


    void Start()
    {
        killCount = 0;
        chargePoint = 0;
        rainbowTime = rainbowTimeMax;
        rainbow.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        rainbowClockAudio.SetActive(false);
    }

    void Update()
    {
        if (!gameActive)
        {
            time += Time.deltaTime;

            if (time > 3.0f) gameActive = true;

        }
        else
        {

            targetEnemy = mainCamera.GetComponent<TouchRayTest>().targetEnemy;
            if (targetEnemy != null)
            {
                var enemyComp = targetEnemy.GetComponent<Enemy>();
                enemyColor = enemyComp.enemyColor.ToString();
                enemyNumber = enemyComp.enemyNumber;
            }

            if (chargePoint >= chargeMax)
            {
                chargePoint = chargeMax;
                rainbowMode = true;
            }


            if (rainbowMode)
            {
                rainbowClockAudio.SetActive(true);
                rainbow.SetActive(true);

                rainbowTime -= Time.deltaTime;

                var digRatio = 1 / rainbowTimeMax;
                var gage = rainbowTime * digRatio;
                chargeGage.fillAmount = gage;

                if (rainbowTime < 0)
                {
                    rainbowMode = false;
                    rainbowTime = rainbowTimeMax;
                    chargePoint = 0;
                }
            }
            else
            {
                rainbowClockAudio.SetActive(false);
                rainbow.SetActive(false);

                var chargeRatio = 1 / chargeMax; //fillAmountのmax1とchargepointのmax100の比率
                var gage = chargePoint * chargeRatio;
                chargeGage.fillAmount = gage;
            }
        }

    }


    public void GetPoint(int point, string color)  //point=チェインした数字の合計,color=色文字列(RED,BLUE,GREEN,YELLOW,BLACK)
    {
        //targetEnemy = mainCamera.GetComponent<TouchRayTest>().targetEnemy;
        if (targetEnemy != null)
        {
            //enemyColor = targetEnemy.GetComponent<Enemy>().enemyColor.ToString();

            if (!rainbowMode)
            {
                if (color == enemyColor)
                {
                    float _point = point * 0.5f;
                    int getPoint = Mathf.RoundToInt(_point); //四捨五入
                    score += getPoint;

                    targetEnemy.GetComponent<Enemy>().enemyNumber -= point;

                    audioSource.PlayOneShot(audioClip[0]);
                    audioSource.PlayOneShot(audioClip[1]);
                }
                else
                {
                    chargePoint += point;

                    float _point = point * 0.1f;
                    int getPoint = Mathf.RoundToInt(_point); //四捨五入
                    score += getPoint;

                    audioSource.PlayOneShot(audioClip[2]);
                }
            }
            else
            {
                float _point = point * 1.0f;
                int getPoint = Mathf.RoundToInt(_point); //四捨五入
                score += getPoint;

                targetEnemy.GetComponent<Enemy>().enemyNumber -= point;

                audioSource.PlayOneShot(audioClip[0]);
                audioSource.PlayOneShot(audioClip[1]);
            }

            
        }
    }

    public void EnemyDestroy(int overKill)
    {
        audioSource.PlayOneShot(audioClip[3]);

        if(overKill == 0)
        {
            overKill = 20;
        }
        score += overKill * 10;
        killCount += 1;
    }


}
