using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject gameMaster;
    //GameObject mainCamera;
    [SerializeField] Text scoreText;
    [SerializeField] Text killText;
    [SerializeField] Text timeText;
    [SerializeField] Text targetText;
    [SerializeField] GameObject readyText;
    [SerializeField] GameObject startText;
    

    float time;

    void Start()
    {
        if (gameMaster == null)
        {
            gameMaster = GameObject.Find("GameMaster");
        }

        time = 0;
    }

    void Update()
    {
        var gameMasterComp = gameMaster.GetComponent<GameMaster>();

        if (gameMasterComp.gameActive)
        {
            time += Time.deltaTime;

            var score = gameMasterComp.score;
            var kill = gameMasterComp.killCount;
            var targetEnemyColor = gameMasterComp.enemyColor;
            var targetEnemyNum = gameMasterComp.enemyNumber;
            var rainbow = gameMasterComp.rainbowMode;

            scoreText.text = string.Format("{0:00000}", score);
            killText.text = string.Format("{0:00}", kill);
            timeText.text = string.Format("{0:000.00}", time);

            if (rainbow) targetText.text = "ALL:" + targetEnemyNum;
            else if (targetEnemyColor == null) targetText.text = "None";
            else targetText.text = targetEnemyColor + ":" + targetEnemyNum;
        }
        else
        {
            time += Time.deltaTime;
            if (time < 3.0f)
            {
                readyText.SetActive(true);
                startText.SetActive(false);
            }
            if (time > 3.0f)
            {
                readyText.SetActive(false);
                startText.SetActive(true);
                time = 0;
            }
        }
    }

    public void ReStart()
    {
        var activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeScene);
;    }
}
