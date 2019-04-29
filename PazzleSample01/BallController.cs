using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject gameMaster;

    public int number { get; private set; }
    int currentNum;

    [SerializeField] float chainRange = 1.0f;
    GameObject[] sameColor;
    List<int> chainBallsNum;
    List<GameObject> chainBallsObj;

    [SerializeField] Text numberText;
    [SerializeField] Text subNumberText;
    Color defColor;
    int defFontsize;
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject destroyParticlePrefab;

    bool chain = false;
    int nearCount = 0;
    int sendPoint = 0;

    public enum BallColor
    {
        RED, BLUE, GREEN, YELLOW, BLACK
    }
    [SerializeField] BallColor ballcolor;

    AudioSource audioSource;
    [SerializeField] AudioClip[] audioClip;

    void Start()
    {
        if (gameMaster == null)
        {
            gameMaster = GameObject.Find("GameMaster");
        }

        sameColor = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
        //パーティクル無効
        var e = particle.emission;
        e.enabled = false;

        //数字ランダム決定
        int randomNumber = Random.Range(1, 10);
        number = randomNumber;
        currentNum = number;

        numberText.text = string.Format("{0}", currentNum);
        defColor = numberText.color;
        defFontsize = numberText.fontSize;

        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {        
        //隣接するボールのナンバーとオブジェクトを初期化
        chainBallsNum = new List<int>();
        chainBallsObj = new List<GameObject>();

        //同色のボール全てを取得
        sameColor = GameObject.FindGameObjectsWithTag(this.gameObject.tag);

        foreach (GameObject i in sameColor)
        {
            //自身と同色ボールそれぞれの距離を測定
            var dis = Vector3.Distance(transform.position, i.transform.position);

            //chainRangeより近い場合chainBallsとしてリストに追加
            if (dis <= chainRange)  //自分自身も含む
            {
                var hitObj = i.gameObject;
                var hitNumber = hitObj.GetComponent<BallController>().number;

                chainBallsNum.Add(hitNumber);
                chainBallsObj.Add(hitObj);

                //Debug.Log(this.gameObject.name + ":" + total);
            }
        }

        //隣接数チェイン判定
        foreach (GameObject i in sameColor)
        {
            var dis = Vector3.Distance(transform.position, i.transform.position);
            if (dis <= chainRange && i != this.gameObject)　//自分自身を除く
            {
                nearCount += 1;

                //二つ以上隣接していたらループ終了
                if (nearCount >= 2)
                {
                    chain = true;
                    break;
                }
            }
            else
            {
                chain = false;
            }
        }

        //チェインの可否によるパーティクル、数字の変更
        if (chain)
        {
            var e = particle.emission;
            e.enabled = true;
            nearCount = 0;

            var sum = chainBallsNum.Sum();
            sendPoint = sum;


            numberText.text = string.Format("{0}", sum);
            numberText.color = new Color(1, 0, 0, 1);
            numberText.fontSize = (int)(defFontsize * 1.5f);
            subNumberText.text = string.Format("{0}", number); 
            //Debug.Log(chainBallsNum.Count + ";" + sum);

        }
        else
        {
            var e = particle.emission;
            e.enabled = false;
            nearCount = 0;


            numberText.text = string.Format("{0}", number);
            numberText.color = defColor;
            numberText.fontSize = defFontsize;
            subNumberText.text = " ";
        }

       

    }

    //長押しによるオブジェクトの消去
    void LongTapped()
    {
        if (chain)
        {
            gameMaster.GetComponent<GameMaster>().GetPoint(sendPoint, ballcolor.ToString());

            var effectPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1.0f);
            Instantiate(destroyParticlePrefab, effectPos, Quaternion.identity);


            //隣接ボールと自身を破壊
            foreach (GameObject i in chainBallsObj)
            {
                Destroy(i);
            }
            //Destroy(gameObject);
        }
    }
    
}

